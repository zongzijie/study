using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using SampleForAspNetCoreMvc.Extensions;
using SampleForAspNetCoreMvc.Service;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class AppServiceModelConvention : IControllerModelConvention
    {
        /// <summary>
        /// 应用模型 AppService约定
        /// 给AppService的每一个方法添加路由属性 api/{controller.ControllerType.FullName}/{action.ActionName}.aspx
        /// 以及谓词约束（GET和POST）
        /// </summary>
        /// <param name="controller"></param>
        public void Apply(ControllerModel controller)
        {
            
            var needHandler=controller.ControllerType.AsType().IsAppService();
            if (needHandler == false)
                return;
            RemoveEmptySelectors(controller.Selectors);
            foreach (var action in controller.Actions)
            {
                
                RemoveEmptySelectors(action.Selectors);
                action.Selectors.Add(
                    new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel(
                            new RouteAttribute(
                                $"api/{controller.ControllerType.FullName}/{action.ActionName}.aspx"
                            )
                        ),
                        ActionConstraints =
                        {
                            new HttpMethodActionConstraint(
                                new[]
                                {
                                    "GET","POST"
                                })
                        }
                    });
                // 打上一个标记，证明此Action是兼容ClownFish的Action
                // 在管道的后期可以根据这个标记精确定位Action，做些想做的事情
                action.Properties["ClownFish"] = "ClownFish";
            }
        }
        protected virtual void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            var emptySelectors = selectors
                .Where(IsEmptySelector)
                .ToList();
            emptySelectors.ForEach(s => selectors.Remove(s));
        }
        protected virtual bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null && (selector.ActionConstraints==null||selector.ActionConstraints.Count<=0);
        }
    }
}