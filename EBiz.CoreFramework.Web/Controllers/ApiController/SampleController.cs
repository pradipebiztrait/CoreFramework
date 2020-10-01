using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
    public class SampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}