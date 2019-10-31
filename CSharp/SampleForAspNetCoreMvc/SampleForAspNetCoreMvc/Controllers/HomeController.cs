using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Logging;
using SampleForAspNetCoreMvc.ApplicationModels;
using SampleForAspNetCoreMvc.Models;

namespace SampleForAspNetCoreMvc.Controllers
{
    [ControllerDescription("wujj")]
    public class HomeController : Controller
    {

       [ActionDescription("wujj")]
        public string Description()
        {
            return ControllerContext.ActionDescriptor.Properties["description"].ToString();
        }
    }
}