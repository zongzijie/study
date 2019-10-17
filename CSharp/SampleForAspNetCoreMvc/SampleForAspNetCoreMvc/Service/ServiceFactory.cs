using System;
using Castle.DynamicProxy;

namespace SampleForAspNetCoreMvc.Service
{
    /// <summary>
    /// 服务工厂。
    /// </summary>
    internal static class ServiceFactory
    {
        /// <summary>
        /// 创建指定类型的Service实例。
        /// </summary>
        /// <typeparam name="T">Service实例的类型。</typeparam>
        /// <returns></returns>
        public static T New<T>() where T : IService
        {
            return (T) NewService(typeof(T));
        }

        /// <summary>
        /// 创建指定类型的Service实例
        /// </summary>
        /// <typeparam name="T"><see cref="IService"/>实例的类型。</typeparam>
        /// <param name="parameters">构造函数参数。</param>
        /// <returns>服务实例。</returns>
        public static T New<T>(params object[] parameters) where T : IService
        {
            return (T) NewService(typeof(T), parameters);
        }


        public static object New(Type type)
        {
            return NewService(type);
        }

        public static object New(Type type, params object[] parameters)
        {
            return NewService(type, parameters);
        }

        private static object NewService(Type type, params object[] parameters)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type instanceType = type;

            //创建实例并返回
            return CreateServiceProxy(instanceType, parameters);
        }


        #region AOP 实现逻辑

        private static readonly IProxyBuilder s_proxyBuilder = new DefaultProxyBuilder(new ModuleScope(false, true));

        private static readonly ProxyGenerationOptions s_Options = new ProxyGenerationOptions(new ProxyGenerationHook());


        /// <summary>
        /// 创建Serivice代理类。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static object CreateServiceProxy(Type type, params object[] parameters)
        {
            PipelineInterceptor interceptor = new PipelineInterceptor();

            ProxyGenerator generator = new ProxyGenerator(s_proxyBuilder);
            IInterceptor[] interceptors = {interceptor};

            return generator.CreateClassProxy(type, s_Options, parameters, interceptors);
        }

        #endregion
    }
}