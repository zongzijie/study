using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class ControllerDescriptionAttribute:Attribute,IControllerModelConvention
    {
        private readonly string _des;
        public ControllerDescriptionAttribute(string des)
        {
            this._des = des;
        }
        public void Apply(ControllerModel controller)
        {
            controller.Properties["description"] = "controller"+_des;
        }
    }
}