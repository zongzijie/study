using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using SampleForAspNetCoreMvc.Extensions;

namespace SampleForAspNetCoreMvc.Service
{
    public class OrderAppService:AppService
    {
        /// <summary>
        /// 获取认购唯一编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrderIdCode(string s,object obj,object obj1)
        {
            return s+obj.ToJson();
        }
        /// <summary>
        /// 获取认购唯一编码
        /// 没有虚方法会报错
        /// </summary>
        /// <returns></returns>
        public  string GetOrderIdCode2(string s,object obj,object obj1)
        {
            return "123";
        }
    }
}