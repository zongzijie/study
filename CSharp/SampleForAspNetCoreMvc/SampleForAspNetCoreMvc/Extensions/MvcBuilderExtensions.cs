using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleForAspNetCoreMvc.ApplicationModels;

namespace SampleForAspNetCoreMvc.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddClownFish(this IMvcBuilder builder)
        {
            //为应用程序部分添加特性，用于告诉应用程序，AppService也是控制器
            builder.PartManager.FeatureProviders.Add(new AppServiceFeatureProvider());
            //替换掉默认的Controller激活器实现
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ClownFishControllerActivator>());
            //ActionIvoker的提供程序，用于在调用Action时进行多参数绑定转换
            builder.Services.AddSingleton<IActionInvokerProvider, ClownFishWebActionInvokerProvider>();
            
            //添加AppService模型约定
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new AppServiceModelConvention());
            });
            return builder;
        }
    }
}