using Castle.DynamicProxy;

namespace SampleForAspNetCoreMvc.Service
{
    public class PipelineInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            
            string before = "before plugin\n ";
            
            invocation.Proceed();
            
            string after = "\nafter plugin";

            invocation.ReturnValue = $"{before} {invocation.ReturnValue} {after}";
        }
    }
}