using System;
using SampleForAspNetCoreMvc.Service;

namespace SampleForAspNetCoreMvc.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAppService(this Type type)
        {
            return typeof(AppService).IsAssignableFrom(type);
        }
    }
}