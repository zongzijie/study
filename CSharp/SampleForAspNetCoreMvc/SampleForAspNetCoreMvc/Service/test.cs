using System.IO.IsolatedStorage;

namespace SampleForAspNetCoreMvc.Service
{
    public class test:IService
    {
        public virtual string Get(string b)
        {
            return b;
        }
    }
}