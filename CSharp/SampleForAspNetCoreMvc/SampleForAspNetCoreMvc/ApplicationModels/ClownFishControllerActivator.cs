using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using SampleForAspNetCoreMvc.Extensions;
using SampleForAspNetCoreMvc.Service;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class ClownFishControllerActivator:IControllerActivator
    {
        
        public virtual object Create(ControllerContext context)
        {
            var type = context.ActionDescriptor.ControllerTypeInfo;

            object instance;
            if (type.IsAppService())
                instance= ServiceFactory.New(type);
            else
                instance = Activator.CreateInstance(type);
            
            return instance;
        }

        public virtual void Release(ControllerContext context, object controller)
        {
            
        }
    }
}