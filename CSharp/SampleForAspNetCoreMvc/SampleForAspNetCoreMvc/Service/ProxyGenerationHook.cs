using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace SampleForAspNetCoreMvc.Service
{
    public class ProxyGenerationHook:IProxyGenerationHook
    {
        // 如果无法拦截，比如没有不是虚方法，则会调用此方法
        // 可以在这里抛出异常，提醒开发者此方法无法被扩展
        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
//                throw new Exception($"{memberInfo.Name}无法被扩展，因为不是虚方法");
        }
        // 判断方法是否需要被拦截，返回false则不会被拦截
        // 这里在二开场景利用事先扫猫程序集生成注册列表后，在这里判断列表中是否有插件来判断是否需要拦截
        // 本示例为了方便理解，只是展示简单的判断
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            if (methodInfo.Name=="GetOrderIdCode")
            {
                return  true;
            }

            return false;
        }

        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public void MethodsInspected()
        {
        }
    }
}