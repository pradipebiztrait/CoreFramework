using System;
using System.Threading.Tasks;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Helper;
using System.Linq;
using EBiz.CoreFramework.DataAccess.Models.AuxiliaryModels;
using System.Collections.Generic;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class UserController : BaseController
    {
        #region 'Declair Variable'
        private readonly IUserService _userService;
		public readonly IEmailService _emailService;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IImageHelper _imageHelper;
        private readonly SiteFolders _siteFolders;
        private readonly SiteSettings _siteSettings;
        private readonly ApplicationDbContext _context;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public UserController(IUserService userService
            , IEmailService emailService
            , IHostingEnvironment appEnvironment
            , IOptions<SiteFolders> siteFolders
            , IOptions<SiteSettings> siteSettings
			, ApplicationDbContext context
			, IImageHelper imageHelper)
        {
			_userService = userService;
            _emailService = emailService;
            _appEnvironment = appEnvironment;
            _siteFolders = siteFolders.Value;
            _siteSettings = siteSettings.Value;
            _imageHelper = imageHelper;
			_context = context;
		}
        #endregion 'Constructor'

        #region 'Get'
        public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Manage(int id)
		{
			var user = new User();

			if (id > 0)
			{
				var result = await _userService.GetUserDetailById(id);

				if (result != null)
				{
					var model = new User();
					//{
					//	user_id = result.user_id,
					//	email = result.email,
					//	user_name = result.user_name,
					//	profile_photo_name = result.profile_photo,
					//	city = result.city,
					//	phone = result.phone,
					//	postal_code = result.postal_code,
					//	gender = result.gender,
					//	member = result.member,
					//	confirmed = result.confirmed,
					//	location = result.location,
					//	active = result.active,
					//	distance = result.distance,
					//	is_lgbtq_flag = result.islgbtq == 1 ? true : false,
					//	meet = result.meet,
					//	profile_photo = !string.IsNullOrEmpty(result.profile_photo) ? result.profile_photo : "/img/default-user-black.svg",
					//};

					return View(model);
				}
				else
				{
					return RedirectToAction("Index");
				}
			}

			return View(user);
		}

        [HttpPost]
        public async Task<JsonResult> GetAll()
        {
			var list = await _userService.GetAllAsync();
			if (list.Data != null)
			{
				foreach (var t in list.Data)
				{
					if (!string.IsNullOrEmpty(t.ImagePath))
					{
						t.ImagePath = t.ImagePath != null ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(t.ImagePath, _siteFolders.UserFolder).Replace("\\", "/") : "/img/default-user-black.svg";
					}
				}
			}
			return Json(new { Data = list.Data });
        }

        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromQuery]FilterQuery request)
        {
            var result = await _userService.ListByFiltersAsync(new FilterHelper().UserFilter(request));

            return Json(new
            {
                rows = result.Data,
                total = result.TotalRecord
            });
        }

        #endregion 'Get'

        #region 'Post'
        [HttpPost]
		public async Task<ApiResponse> SaveAsync()
		{
			var _dataRes = new ApiResponse();
			var model = JsonConvert.DeserializeObject<User>(HttpContext.Request.Form["model"][0]);

			//var files = HttpContext.Request.Form.Files;
			//string Webrootpath = _appEnvironment.WebRootPath;

			var response = await _userService.SaveUserAsync(model);

			//if (files.Count != 0 && response.Status)
			//{
			//	var guid = !string.IsNullOrEmpty(model.profile_photo_name) ? Path.GetFileNameWithoutExtension(model.profile_photo_name) : Guid.NewGuid().ToString();
			//	_imageHelper.SaveImage(files, guid, _siteFolders.UserFolder);
			//	var extension = Path.GetExtension(files[0].FileName);
			//	_userRepository.UpdateUserImagePath(response.ReturnId, guid + extension);
			//}

			_dataRes.Status = response.Status;
			_dataRes.Message = response.Message;

			return _dataRes;
		}

		[HttpPost]
        public async Task<ApiResponse> IsActiveUser(Int64 userId, int isActive)
        {
            return await _userService.IsActiveUser(userId, isActive);
        }

        [HttpPost]
        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _userService.DeleteAsync(id);

        [HttpPost]
        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _userService.DeleteMultipleAsync(ids);

        #endregion 'Post'
    }
}