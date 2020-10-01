using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBiz.CoreFramework.Web.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EBiz.CoreFramework.Web.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}