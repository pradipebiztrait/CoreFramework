using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.Infrastructure.Helper;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class CmsController : BaseController
	{
		#region 'Declair Variable'
		private readonly ICmsService _cmsService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public CmsController(ICmsService cmsService)
		{
			_cmsService = cmsService;
		}
		#endregion 'Constructor'

		#region 'Get'
		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResult> GetAll()
		{
			var list = await _cmsService.GetAllAsync();
			return Json(new { data = list.Data });
		}

		public async Task<IActionResult> Manage(int id)
		{
			var mdl = new Pages();
			if (id > 0)
			{
				var result = await _cmsService.GetAsync(id);

				if(result.Data != null)
				{
					var model = new Pages()
					{
						PageId = result.Data.PageId,
						PageTitle = result.Data.PageTitle,
						PageUrl = result.Data.PageUrl,
						PageDescription = result.Data.PageDescription,
						is_status = (result.Data.IsActive == 1 ? true : false)
					};

					return View(model);
				}
				
			}

			return View(mdl);

		}
        #endregion 'Get'

        #region 'Post'
        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromQuery]FilterQuery request)
        {
            var result = await _cmsService.GetAllPageByFilterAsync(new FilterHelper().PageFilter(request));

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
				var model = JsonConvert.DeserializeObject<Pages>(HttpContext.Request.Form["model"][0]);
				model.UserId = CurrentUser.UserId;

				var result = await _cmsService.SaveAsync(model);
				
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
        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _cmsService.DeleteAsync(id);

        [HttpPost]
        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _cmsService.DeleteMultipleAsync(ids);

        #endregion 'Post'

        #region 'ViewComponent'
        [HttpGet]
        public IActionResult ManagePageComponent(Int64 id)
        {
            return ViewComponent("ManagePageComponent", new { id = id });
        }
        #endregion 'ViewComponent'
    }
}
