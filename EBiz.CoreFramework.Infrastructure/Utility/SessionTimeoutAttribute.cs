using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Web.Routing;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
	public class SessionTimeoutAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext _fc)
		{
   //         if (string.IsNullOrEmpty(_fc.HttpContext.Session.GetString("_isAdmin"))
			//	|| _fc.HttpContext.Session.GetString("_isAdmin") != "1"
			//	|| string.IsNullOrEmpty(_fc.HttpContext.Session.GetString("_userRole"))
			//	|| _fc.HttpContext.Session.GetString("_userId") == null)
			//{
			//	_fc.Result = new RedirectResult("~/Admin/Login");
			//	return;
			//}
            //string name = (string)_fc.RouteData.Values["Controller"];
            //if (_fc.HttpContext.Session.GetString("_lockscreen") == "1")
            //{
            //    _fc.Result = new RedirectResult("~/Admin/Lockscreen");
            //    return;
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        private void Log(string methodName, RouteData routeData)
        {
            
        }
    }
}
