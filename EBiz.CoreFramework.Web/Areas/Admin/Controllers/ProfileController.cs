using System;
using System.IO;
using System.Threading.Tasks;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EBiz.CoreFramework.Service.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class ProfileController : BaseController
    {
        #region 'Declair Variable'
        private readonly IUserService _userService;
        private readonly IImageHelper _imageHelper;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly SiteFolders _siteFolders;
        private readonly SiteSettings _siteSettings;
        #endregion 'Declair Variable'


        #region 'Constructor'
        public ProfileController(IUserService userService
			, IOptions<SiteFolders> siteFolders
            , IOptions<SiteSettings> siteSettings
            , IImageHelper imageHelper
            , IHostingEnvironment appEnvironment)
        {
			_userService = userService;
            _siteFolders = siteFolders.Value;
            _siteSettings = siteSettings.Value;
            _imageHelper = imageHelper;
            _appEnvironment = appEnvironment;
        }
        #endregion 'Constructor'

        #region 'Get'
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "Dashboard";
			var user = _userService.GetById(CurrentUser.UserId).Result;

			var model = new UserProfile()
            {
				UserId = user.UserId,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.EmailAddress,
				MobileNumber = user.MobileNumber,
				ImageName = user.ImagePath,
				ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(user.ImagePath, _siteFolders.UserFolder).Replace("\\", "/") : user.ImagePath
			};

            return View(model);
        }
        #endregion 'Get'

        #region 'Post'
        [HttpPost]
        public async Task<ApiResponse> SaveAsync()
        { 
            var _apiResponse = new ApiResponse();
            var model = JsonConvert.DeserializeObject<UserProfile>(HttpContext.Request.Form["model"][0]);

            var files = HttpContext.Request.Form.Files;
            string Webrootpath = _appEnvironment.WebRootPath;

            try
            {
                if (!string.IsNullOrEmpty(model.ImagePath))
                {
                    model.ImagePath = model.ImageName;
                }
                var result = await _userService.SaveUserProfileAsync(model);

                if (!string.IsNullOrEmpty(model.ImagePath))
                {
                    model.ImagePath = _siteSettings.BaseUrl + _imageHelper.GetImagePath(model.ImageName, _siteFolders.UserFolder).Replace("\\", "/");
                }

                if (result.Status && files.Count > 0)
                {
                    var guid = !string.IsNullOrEmpty(model.ImageName) ? Path.GetFileNameWithoutExtension(model.ImageName) : Guid.NewGuid().ToString();
                    model.ImagePath = _imageHelper.SaveImage(files[0], guid, "User", model.ImageName);
                    _userService.UpdateUserImagePath(model.UserId, guid + Path.GetExtension(model.ImagePath));

                    model.ImagePath = _siteSettings.BaseUrl + '/' + model.ImagePath.Replace("\\", "/");
                    //model.ImageName = guid + Path.GetExtension(model.ImagePath);
                    HttpContext.Session.SetString("_userImage", model.ImagePath);
                }

                _apiResponse.Status = true;
                _apiResponse.Message = result.Message;
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.Message = ex.Message.ToString();
            }
            return _apiResponse;
        }
        #endregion 'Post'
    }
}