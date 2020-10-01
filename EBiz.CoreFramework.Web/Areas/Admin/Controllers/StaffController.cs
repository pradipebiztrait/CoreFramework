using System;
using System.Threading.Tasks;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Enum;
using EBiz.CoreFramework.Infrastructure.Utility;
using EBiz.CoreFramework.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	[AuthorizeRole(RoleType.ADMIN)]
	public class StaffController : BaseController
    {
        #region 'Declair Variable'
        public RolePermission _permission;
        private readonly IStaffService _staffService;
        public readonly IEmailService _emailService;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IImageHelper _imageHelper;
        private readonly IRoleService _roleService;
        private readonly SiteFolders _siteFolders;
        private readonly SiteSettings _siteSettings;
        private readonly ApplicationDbContext _context;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public StaffController(IStaffService staffService
            , IEmailService emailService
            , IHostingEnvironment appEnvironment
            , IOptions<SiteFolders> siteFolders
            , IOptions<SiteSettings> siteSettings
            , ApplicationDbContext context
            , IImageHelper imageHelper
            , IRoleService roleService)
        {
            _staffService = staffService;
            _emailService = emailService;
            _appEnvironment = appEnvironment;
            _siteFolders = siteFolders.Value;
            _siteSettings = siteSettings.Value;
            _imageHelper = imageHelper;
            _context = context;
            _roleService = roleService;
        }
        #endregion 'Constructor'

        #region 'Get'
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromQuery]FilterQuery request)
        {
            var result = await _staffService.ListByFiltersAsync(new FilterHelper().UserFilter(request));

            return Json(new
            {
                rows = result.Data,
                total = result.TotalRecord
            });
        }

        [HttpGet]
        public IActionResult IsExistEmailAddress(string EmailAddress)
        {
            try
            {
                var isExist = false;
                var user = _context.users.Where(t => t.EmailAddress.Equals(EmailAddress)).FirstOrDefault();
                if (user != null) isExist = true;

                return Json(new { valid = !isExist });
            }
            catch (Exception ex)
            {
                return Json(new { valid = false, message = ex.Message.ToString() });
            }
           
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

            var response = await _staffService.SaveStaffAsync(model);

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
        public async Task<ApiResponse> IsActiveUser(Int64 userId, int isActive) => await _staffService.IsActiveUser(userId, isActive);

        [HttpPost]
        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _staffService.DeleteAsync(id);

        [HttpPost]
        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _staffService.DeleteMultipleAsync(ids);

        #endregion 'Post'

        #region 'ViewComponent'
        [HttpGet]
        public IActionResult ManageComponent(Int64 id)
        {
            return ViewComponent("ManageStaffComponent", new { id = id });
        }
        #endregion 'ViewComponent'

        #region 'Init'
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _permission = _roleService.PermissionAsync(_role, _menu).Result;
            if (_permission.is_view == 0) filterContext.Result = new RedirectResult("/Admin/Error/403");
        }
        #endregion
    }
}