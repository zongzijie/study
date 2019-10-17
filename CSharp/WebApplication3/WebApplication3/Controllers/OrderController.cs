using System;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class OrderController : ApiController
    {
        [HttpGet]
        public string GetOrder()
        {
            return "Success";
        }
    }
}