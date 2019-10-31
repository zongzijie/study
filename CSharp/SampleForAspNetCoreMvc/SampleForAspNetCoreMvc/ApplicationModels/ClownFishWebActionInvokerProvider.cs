using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class ClownFishWebActionInvokerProvider : IActionInvokerProvider
    {
        private readonly ControllerActionInvokerCache _controllerActionInvokerCache;
        private readonly IReadOnlyList<IValueProviderFactory> _valueProviderFactories;
        private readonly int _maxModelValidationErrors;
        private readonly ILogger _logger;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly IActionResultTypeMapper _mapper;
        public ClownFishWebActionInvokerProvider(
            ControllerActionInvokerCache controllerActionInvokerCache,
            IOptions<MvcOptions> optionsAccessor,
            DiagnosticSource diagnosticSource,
            IActionResultTypeMapper mapper)
        {
            this._controllerActionInvokerCache = controllerActionInvokerCache;
            this._valueProviderFactories = (IReadOnlyList<IValueProviderFactory>) optionsAccessor.Value.ValueProviderFactories.ToArray<IValueProviderFactory>();
            this._maxModelValidationErrors = optionsAccessor.Value.MaxModelValidationErrors;
            this._diagnosticSource = diagnosticSource;
            this._mapper = mapper;
        }
        /// <summary>
        /// 提供程序执行完成事件
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
        }

        // 最先执行
        public int Order => -2000;
        /// <summary>
        /// 提供程序执行时调用
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            // 只针对控制器的Action进行操作
            if (!(context.ActionContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return;
            }

            // 只对ClownFish的Action进行参数绑定处理
            if (controllerActionDescriptor.Properties.ContainsKey("ClownFish")==false)
            {
                return;
            }
            
            ControllerContext controllerContext = new ControllerContext(context.ActionContext);
            controllerContext.ValueProviderFactories =new CopyOnWriteList<IValueProviderFactory>(_valueProviderFactories);
            controllerContext.ModelState.MaxAllowedErrors = _maxModelValidationErrors;
            var cachedResult = _controllerActionInvokerCache.GetCachedResult(controllerContext);
            var controllerActionInvokerCacheEntry = cachedResult.cacheEntry;
            if (controllerActionInvokerCacheEntry!=null)
            {
                //兼容多参数绑定的实现
                CompatibilityClownFishWeb(controllerActionDescriptor,
                    controllerActionInvokerCacheEntry);
            }
        }
        //兼容ClownFish的多参数绑定
        private void CompatibilityClownFishWeb(
            ControllerActionDescriptor actionDescriptor,
            ControllerActionInvokerCacheEntry controllerActionInvokerCacheEntry)
        {
            // 合并参数绑定委托
            var controllerBinderDelegate = MergeControllerBinderDelegate(Bind,
                controllerActionInvokerCacheEntry.ControllerBinderDelegate);
            
//            var objectMethodExecutor = controllerActionInvokerCacheEntry?.GetType()
//                .GetProperty("ObjectMethodExecutor")?.GetValue(controllerActionInvokerCacheEntry);
//            var actionMethodExecutor = controllerActionInvokerCacheEntry?.GetType()
//                .GetProperty("ActionMethodExecutor")?.GetValue(controllerActionInvokerCacheEntry);
            
            // 替换ControllerActionInvokerCacheEntry中的参数绑定委托
            controllerActionInvokerCacheEntry=ReplaceCacheEntry(controllerActionInvokerCacheEntry,controllerBinderDelegate);
            
//            object obj=Activator.CreateInstance(typeof(ControllerActionInvokerCacheEntry), BindingFlags.Instance | BindingFlags.NonPublic,
//                null, new object[]
//                {
//                    controllerActionInvokerCacheEntry.CachedFilters,
//                    controllerActionInvokerCacheEntry.ControllerFactory,
//                    controllerActionInvokerCacheEntry.ControllerReleaser,
//                    controllerBinderDelegate,
//                    objectMethodExecutor,
//                    actionMethodExecutor
//                }, null);
            
            // 改写缓存，新的缓存中使用新的参数绑定委托，支持多参数绑定
            var cache = GetCache();
            cache[actionDescriptor] = controllerActionInvokerCacheEntry;
        }
        
        // 反射获取缓存记录集合，用于直接修改缓存中的参数绑定委托
        private ConcurrentDictionary<ActionDescriptor, ControllerActionInvokerCacheEntry> GetCache()
        {
            var currentCache = _controllerActionInvokerCache?.GetType()
                .GetProperty("CurrentCache", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(_controllerActionInvokerCache);

            return (ConcurrentDictionary<ActionDescriptor, ControllerActionInvokerCacheEntry>) currentCache?.GetType()
                .GetProperty("Entries", BindingFlags.Instance | BindingFlags.Public)?.GetValue(currentCache);
        }
        // 合并参数绑定委托，检查Body类型如果是json则使用多参数绑定委托，如果不是，则使用mvc提供的默认委托
        private ControllerBinderDelegate MergeControllerBinderDelegate(ControllerBinderDelegate clownFishWebDelegate,
            ControllerBinderDelegate mvcCoreDelegate)
        {
            return (controllerContext, controller, arguments) =>
            {
                var contentType = controllerContext.HttpContext.Request.ContentType;


                if (string.IsNullOrEmpty(contentType) == false &&
                    contentType.IndexOf("application/json", StringComparison.OrdinalIgnoreCase) >= 0)
                    return clownFishWebDelegate(controllerContext, controller, arguments);
                else
                    return mvcCoreDelegate(controllerContext, controller, arguments);
            };
        }
        /// <summary>
        /// 替换cacheEntry;
        /// </summary>
        /// <param name="cacheEntry"></param>
        /// <param name="binderDelegate"></param>
        /// <returns></returns>
        private static ControllerActionInvokerCacheEntry ReplaceCacheEntry(ControllerActionInvokerCacheEntry cacheEntry,
            ControllerBinderDelegate binderDelegate)
        {
            return s_getCacheEntryDelegate.Value(cacheEntry, binderDelegate);
        }

        private static
            Lazy<Func<ControllerActionInvokerCacheEntry, ControllerBinderDelegate, ControllerActionInvokerCacheEntry>>
            s_getCacheEntryDelegate =
                new Lazy<Func<ControllerActionInvokerCacheEntry, ControllerBinderDelegate,
                    ControllerActionInvokerCacheEntry>>(GetCacheEntryFactory,
                    LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// 生成调用ControllerActionInvokerCacheEntry的表达式
        /// </summary>
        /// <returns></returns>
        private static
            Func<ControllerActionInvokerCacheEntry, ControllerBinderDelegate, ControllerActionInvokerCacheEntry>
            GetCacheEntryFactory()
        {
            var controllerActionInvokerCacheEntryType = typeof(ControllerActionInvokerCacheEntry);

            var controllerBinderDelegateType = typeof(ControllerBinderDelegate);
            //var controllerBinderDelegatePropertyInfo = controllerActionInvokerType.GetProperty(nameof(ControllerActionInvokerCacheEntry.ControllerBinderDelegate), BindingFlags.Instance | BindingFlags.Public);
            
            // 找到参数最多的那个构造函数 
            var constructorInfo = controllerActionInvokerCacheEntryType
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(e => e.GetParameters().Length).First();
            
            // 拿到构造函数的参数定义列表
            var parameterInfos = constructorInfo.GetParameters();
            
            // 创建稍后的Lambda表达式所需的参数表达式
            var oldInstanceParameterExpression = Expression.Parameter(controllerActionInvokerCacheEntryType,
                "ControllerActionInvokerCacheEntry");
            var delegateParameterExpression =
                Expression.Parameter(controllerBinderDelegateType, "controllerBinderDelegate");

            // 准备构造函数所需的参数表达式列表
            var memberExpressions = new List<Expression>(parameterInfos.Length);

            // 扫描缓存记录类型的所有运行时属性，用于循环构建构造函数所需的参数定义列表
            var propertyInfos = controllerActionInvokerCacheEntryType.GetRuntimeProperties().ToArray();

            // 循环填充构造函数参数表达式列表
            foreach (var parameterInfo in parameterInfos)
            {
                var propertyInfo = propertyInfos.Single(e => e.PropertyType == parameterInfo.ParameterType);

                if (propertyInfo.PropertyType == controllerBinderDelegateType)
                {
                    memberExpressions.Add(delegateParameterExpression);
                }
                else
                {
                    var memberExpression = Expression.Property(oldInstanceParameterExpression, propertyInfo);
                    memberExpressions.Add(memberExpression);
                }
            }
            // 创建"使用指定参数列表调用指定构造函数"的表达式树
            var newInstanceExpression = Expression.New(constructorInfo, memberExpressions);
            // 编译Lambda表达式
            return Expression
                .Lambda<Func<ControllerActionInvokerCacheEntry, ControllerBinderDelegate,
                    ControllerActionInvokerCacheEntry>>(
                    newInstanceExpression,
                    oldInstanceParameterExpression, delegateParameterExpression).Compile();
        }
        private  async Task Bind(ControllerContext controllerContext, object controller,
            Dictionary<string, object> arguments)
        {
            var actionDescriptor = controllerContext.ActionDescriptor;
            var parameterDescriptors= actionDescriptor.Parameters;
            var request = controllerContext.HttpContext.Request;
            var jsonSerializer = JsonSerializer.CreateDefault();
            using (var streamReader=new HttpRequestStreamReader(request.Body,Encoding.UTF8))
            {
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    //json相关类型处理handler
                    jsonTextReader.DateTimeZoneHandling = jsonSerializer.DateTimeZoneHandling;
                    jsonTextReader.DateParseHandling = jsonSerializer.DateParseHandling;
                    jsonTextReader.FloatParseHandling = jsonSerializer.FloatParseHandling;
                    jsonTextReader.DateFormatString = jsonSerializer.DateFormatString;
                    jsonTextReader.Culture = jsonSerializer.Culture;

                    while (jsonTextReader.Read())
                    {
                        if (jsonTextReader.TokenType == JsonToken.PropertyName)
                        {
                            if (jsonTextReader.Depth != 1)
                            {
                                throw new InvalidOperationException("提交的数据格式错误");
                            }

                            string propertyName = (string) jsonTextReader.Value;
                            
                            //根据参数名称从所有参数信息中获取到该参数信息
                            var parameterDescriptor = parameterDescriptors.SingleOrDefault(input =>
                                string.Equals(input.Name, propertyName, StringComparison.OrdinalIgnoreCase));
                            if (parameterDescriptor == null)
                            {
                                //忽略在Json中存在的参数，但是在方法的参数列表中不存在。
                                jsonTextReader.Read();
                                while (jsonTextReader.Depth != 1)
                                    jsonTextReader.Read();
                            }
                            else
                            {
                                Type parameterType = parameterDescriptor.ParameterType;

                                //如果参数是string类型可以直接获取，否则需要反序列化为相应对象
                                if (parameterType == typeof(string))
                                {
                                    arguments[parameterDescriptor.Name] = jsonTextReader.ReadAsString();
                                }
                                else
                                {
                                    jsonTextReader.Read();
                                    arguments[parameterDescriptor.Name] =
                                        jsonSerializer.Deserialize(jsonTextReader, parameterType);
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}