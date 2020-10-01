using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	[AuthorizeRole(RoleType.ADMIN, RoleType.SUBADMIN)]
    public class LockscreenController : Controller
    {

		#region 'Declair Variable'
		private readonly IDashboardService _dashboardService;
		private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly SiteSettings _siteSettings;
        private readonly SiteFolders _siteFolders;
        private readonly IImageHelper _imageHelper;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public LockscreenController(
            IDashboardService dashboardService
            , IUserService userService
            , ITokenService tokenService
            , IOptions<SiteSettings> siteSettings
            , IImageHelper imageHelper
            , IOptions<SiteFolders> siteFolders)
		{
            _userService = userService;
            _siteSettings = siteSettings.Value;
            _imageHelper = imageHelper;
            _siteFolders = siteFolders.Value;
            _tokenService = tokenService;
        }
		#endregion 'Constructor'

		public IActionResult Index()
        {
            var result = _userService.GetById(Convert.ToInt64(HttpContext.Session.GetString("_userId")));
            var user = new LockscreenLogin()
            {
                FirstName = result.Result.FirstName,
                LastName = result.Result.LastName,
                Email = result.Result.EmailAddress,
            };

            if (user != null)
            {
                user.ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(user.ImagePath, _siteFolders.UserFolder) : "/img/default-user-black.svg";
            }

            HttpContext.Session.SetString("_lockscreen", "1");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LockscreenLogin model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                TempData["ErrorMessage"] = "Invalid password.";
                return RedirectToAction("Index", "Lockscreen", new { area = "Admin" });
            }
            var data = await _tokenService.TokenGenerate(model.Email, model.Password);

            if (data.Item1 != null && data.Item3.Status && data.Item2.IsAdmin == 1)
            {
                HttpContext.Session.SetString("JWToken", data.Item1);
                HttpContext.Session.SetString("_userName", data.Item2.EmailAddress);
                HttpContext.Session.SetString("_userId", data.Item2.UserId.ToString());
                HttpContext.Session.SetString("_roleId", data.Item2.RoleId.ToString());
                HttpContext.Session.SetString("_userImage", !string.IsNullOrEmpty(data.Item2.ImagePath) ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(data.Item2.ImagePath, _siteFolders.UserFolder) : "/img/default-user-black.svg");
                HttpContext.Session.SetString("_isAdmin", "1");
                HttpContext.Session.SetObjectAsJson("CurrentUser", data.Item2);

                if (data.Item2.IsAdmin == 1)
                {
                    HttpContext.Session.SetString("_userRole", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid password.";
                return RedirectToAction("Index", "Lockscreen", new { area = "Admin" });
            }

            HttpContext.Session.SetString("_lockscreen", "0");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        #region 'ViewComponent'

        #endregion 'ViewComponent'
    }
}