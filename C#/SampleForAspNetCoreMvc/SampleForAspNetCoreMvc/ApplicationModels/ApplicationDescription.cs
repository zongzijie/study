using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SampleForAspNetCoreMvc.ApplicationModels
{
    public class ApplicationDescription:IApplicationModelConvention
    {
        private readonly string _des;

        public ApplicationDescription(string des)
        {
            this._des = des;
        }
        
        public void Apply(ApplicationModel application)
        {
            application.Properties["description"] = "Application"+this._des;
        }
    }
}