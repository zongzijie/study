using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace SampleForAspNetCoreMvc.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var sb = new StringBuilder();
            var h=JsonSerializer.CreateDefault();
            h.Serialize(new StringWriter(sb),obj );
            return sb.ToString();
        }
    }
}