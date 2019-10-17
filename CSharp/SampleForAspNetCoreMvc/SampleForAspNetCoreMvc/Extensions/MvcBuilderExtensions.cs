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
            builder.PartManager.FeatureProviders.Add(new AppServiceFeatureProvider());
            
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ClownFishControllerActivator>());
            
            builder.Services.AddSingleton<IActionInvokerProvider, ClownFishWebActionInvokerProvider>();
            
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new AppServiceModelConvention());
            });
            return builder;
        }
    }
}