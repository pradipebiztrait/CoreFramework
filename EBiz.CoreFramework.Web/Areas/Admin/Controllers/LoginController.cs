using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using EBiz.CoreFramework.Web.Utility;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Enum;
using EBiz.CoreFramework.Service.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace EBiz.CoreFramework.Web.AdminControllers
{
	[Area("Admin")]
    public class LoginController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly SiteSettings _siteSettings;
        private readonly SiteFolders _siteFolders;
        private readonly ApplicationDbContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IMenuService _menuService;
        private readonly IMemoryCache _cache;

        public LoginController(
			ITokenService tokenService
            , IOptions<SiteSettings> siteSettings
            , ApplicationDbContext context
            , IImageHelper imageHelper
            , IOptions<SiteFolders> siteFolders
            , IMenuService menuService
            , IMemoryCache cache
            )
        {
            _tokenService = tokenService;
            _siteSettings = siteSettings.Value;
            _context = context;
            _imageHelper = imageHelper;
            _siteFolders = siteFolders.Value;
            _menuService = menuService;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!(Convert.ToInt64(HttpContext.Session.GetString("_userId")) > 0) || HttpContext.Session.GetString("_isAdmin") != "1")
            {
                HttpContext.Session.Clear();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminLogin model)
        {
            try
            {
                _cache.Remove(CacheKeys.Menu);
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return RedirectToAction("Index", "Login", new { area = "Admin" });
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
                    HttpContext.Session.SetString("_lockscreen", "0");
                    HttpContext.Session.SetObjectAsJson("CurrentUser", data.Item2);

                    if (data.Item2.IsAdmin == 1)
                    {
                        HttpContext.Session.SetString("_userRole", "Admin");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return RedirectToAction("Index", "Login", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            
        }
    }
}