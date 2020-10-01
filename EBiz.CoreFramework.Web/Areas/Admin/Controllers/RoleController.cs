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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class RoleController : BaseController
	{
		#region 'Declair Variable'
		private readonly IRoleService _roleService;
        public RolePermission _permission;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public RoleController(IRoleService roleService)
		{
            _roleService = roleService;
        }
		#endregion 'Constructor'

		#region 'Get'
		public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> Manage(int id)
        {
            var model = new Roles();
            if (id > 0)
            {
                var result = await _roleService.GetAsync(id);

                if (result.Data != null)
                {
                    var editModel = new Roles()
                    {
                        role_id = result.Data.role_id,
                        role_name = result.Data.role_name
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
            var result = await _roleService.GetAllByFilterAsync(new FilterHelper().RoleFilter(request));

            return Json(new
            {
                rows = result.Data,
                total = result.TotalRecord
            });
        }

        [HttpPost]
        public async Task<ApiResponse> GetUserPermissionAsync(Int64 id) => await _roleService.GetUserPermissionAsync(id);

        [HttpPost]
        public async Task<ApiResponse> SaveAsync()
        {
            var response = new ApiResponse();
            try
            {
                var model = JsonConvert.DeserializeObject<Roles>(HttpContext.Request.Form["model"][0]);
                model.UserId = CurrentUser.UserId;

                var result = await _roleService.SaveAsync(model);

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
        public async Task<ApiResponse> SaveRolePermissionAsync()
        {
            var response = new ApiResponse();
            try
            {
                var model = JsonConvert.DeserializeObject<List<RolePermission>>(HttpContext.Request.Form["model"][0]);

                var result = await _roleService.SaveRolePermissionAsync(model);

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
            return await _roleService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
            return await _roleService.DeleteMultipleAsync(ids);
        }

        #endregion 'Post'

        #region 'ViewComponent'
        [HttpGet]
        public IActionResult ManageComponent(Int64 id)
        {
            return ViewComponent("ManageRoleComponent", new { id = id });
        }

        [HttpGet]
        public IActionResult ManageRolePermissionComponent(Int64 id)
        {
            return ViewComponent("ManageRolePermissionComponent", new { id = id });
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
