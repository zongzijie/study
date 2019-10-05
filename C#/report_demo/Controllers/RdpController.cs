using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using report_demo.Models;

namespace report_demo.Controllers
{
    public class RdpController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RdpController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult> GetRdpReportHtml(string rdpGuid, Dictionary<string,string> paramList)
        {
            var client = _httpClientFactory.CreateClient();
//            var url = $"http://localhost:8080/RDP-SERVER/rdppub/show?uuid=d65fcdaf6141ca869573b96fa4f93869&opt=previewNew&currentPage=1&pageSize=0&pageType=1&totalRecord=0";
           var url = $"http://localhost:8080/RDP-SERVER/rdppub/show?uuid={rdpGuid}&opt=previewNew&currentPage={paramList["currentPage"]}&pageSize={paramList["pageSize"]}&pageType=1&totalRecord={paramList["totalRecord"]}";
            var list=paramList.Select(p => $"{p.Key}={p.Value}");
            var param=string.Join('&', list);
            var content=new StringContent(param);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var result = await client.PostAsync(url, content);
            return Ok(Json(result.Content.ReadAsStringAsync()));
        }
    }
}