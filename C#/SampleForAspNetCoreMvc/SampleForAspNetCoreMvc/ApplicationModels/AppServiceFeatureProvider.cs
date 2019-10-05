using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using SampleForAspNetCoreMvc.Service;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class AppServiceFeatureProvider:ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeof(AppService).IsAssignableFrom(typeInfo.AsType());
            //return typeInfo.IsClass; 
        }
    }
}