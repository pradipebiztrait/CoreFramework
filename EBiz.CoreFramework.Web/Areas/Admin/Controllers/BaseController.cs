using System;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.ActionFilters;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBiz.CoreFramework.Web.Controllers.AdminControllers
{
	//[SessionTimeout]
	[TypeFilter(typeof(CustomExceptionFilter))]
	[Area("Admin")]
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        public bool IsAuthenticate()
        {
            try
            {
                if (CurrentUserId() > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64? CurrentUserId()
        {
            return Convert.ToInt64(HttpContext.Session.GetString("_userId"));
        }

        public string CurrentUserRole()
        {
            return HttpContext.Session.GetString("_userRole");
        }

        public User cUser()
        {
            return HttpContext.Session.GetObjectFromJson<User>("CurrentUser");
        }

        public User CurrentUser { get { return cUser(); } }

        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public Int64 _role { get { return Convert.ToInt64(HttpContext.Session.GetString("_roleId")); } }

        public string _menu { get { return this.ControllerContext.RouteData.Values["controller"].ToString(); } }
    }
}