using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Helper;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class MenuController : BaseController
	{
		#region 'Declair Variable'
		private readonly IMenuService _menuService;
        private readonly IMemoryCache _cache;
        private readonly IRoleService _roleService;
        public RolePermission _permission;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public MenuController(IMenuService menuService, IMemoryCache memoryCache, IRoleService roleService)
		{
            _menuService = menuService;
            _cache = memoryCache;
            _roleService = roleService;
        }
		#endregion 'Constructor'

		#region 'Get'
		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResult> GetAll()
		{
			var list = await _menuService.GetAllParentMenuAsync();
			return Json(new { data = list.Data });
		}

        public async Task<IActionResult> Manage(int id)
        {
            var model = new Menu();
            if (id > 0)
            {
                var result = await _menuService.GetAsync(id);

                if (result.Data != null)
                {
                    var editModel = new Menu()
                    {
                        menu_id = result.Data.menu_id,
                        parent_menu_id = result.Data.parent_menu_id,
                        menu_title = result.Data.menu_title,
                        menu_url = result.Data.menu_url,
                        menu_icon = result.Data.menu_icon,
                        is_status = (result.Data.is_active == 1 ? true : false)
                    };

                    return View(editModel);
                }
            }

            return View(model);

        }
        #endregion 'Get'

        #region 'Post'
        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromQuery]FilterQuery request)
        {
            var result = await _menuService.GetAllByFilterAsync(new FilterHelper().MenuFilter(request));

            return Json(new
            {
                rows = result.Data,
                total = result.TotalRecord
            });
        }

        [HttpPost]
        public async Task<ApiResponse> SaveAsync()
        {
            var response = new ApiResponse();
            try
            {
                _cache.Remove(CacheKeys.Menu);
                var model = JsonConvert.DeserializeObject<Menu>(HttpContext.Request.Form["model"][0]);
                model.UserId = CurrentUser.UserId;

                var result = await _menuService.SaveAsync(model);

                response.Status = result.Status;
                response.Message = result.Message;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message.ToString();
            }

            return response;
        }

        [HttpPost]
        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            _cache.Remove(CacheKeys.Menu);
            return await _menuService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
            _cache.Remove(CacheKeys.Menu);
            return await _menuService.DeleteMultipleAsync(ids);
        }

        #endregion 'Post'

        #region 'ViewComponent'
        [HttpGet]
        public IActionResult ManageComponent(Int64 id)
        {
            return ViewComponent("ManageMenuComponent", new { id = id });
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
