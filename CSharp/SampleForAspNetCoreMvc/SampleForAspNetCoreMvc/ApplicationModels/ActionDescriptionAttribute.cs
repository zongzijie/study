using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    
    public class ActionDescriptionAttribute:Attribute,IActionModelConvention
    {
        private readonly string _des;

        public ActionDescriptionAttribute(string des)
        {
            this._des = des;
        }
        
        public void Apply(ActionModel action)
        {
            action.Properties["description"] ="action"+ _des;
        }
    }
}