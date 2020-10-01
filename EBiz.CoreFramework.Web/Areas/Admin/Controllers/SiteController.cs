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
using EBiz.CoreFramework.DataAccess.DbContext;
using System.Linq;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class SiteController : BaseController
	{
		#region 'Declair Variable'
		private readonly ISiteSettingService _service;
		private readonly ApplicationDbContext _context;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public SiteController(ISiteSettingService service, ApplicationDbContext context)
		{
            _service = service;
            _context = context;
        }
		#endregion 'Constructor'

		#region 'Get'
		public async Task<IActionResult> Index() => View(await _service.GetSiteSettingsAsync());

        #endregion 'Get'

        #region 'Post'

        [HttpPost]
        public async Task<ApiResponse> SaveEmailSettingsAsync()
        {
            var response = new ApiResponse();
            try
            {
                var model = JsonConvert.DeserializeObject<SiteEmail>(HttpContext.Request.Form["model"][0]);
                model.UserId = CurrentUser.UserId;

                var result = await _service.SaveEmailSettingsAsync(model);

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
        public void IsActiveEmailSetting(int status)
        {
            var model = _context.site_email.FirstOrDefault();
            model.is_active = status;

            _context.site_email.Update(model);
            _context.SaveChanges();
        }

        [HttpPost]
        public void IsActiveNotification(int status)
        {
            var model = _context.site_notification.FirstOrDefault();
            model.is_active = status;

            _context.site_notification.Update(model);
            _context.SaveChanges();
        }

        [HttpPost]
        public async Task<ApiResponse> SavePushNotificationSettingsAsync()
        {
            var response = new ApiResponse();
            try
            {
                var model = JsonConvert.DeserializeObject<SiteNotification>(HttpContext.Request.Form["model"][0]);
                model.UserId = CurrentUser.UserId;

                var result = await _service.SavePushNotificationSettingsAsync(model);

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
        public async Task<ApiResponse> SaveAWSPropertySettingsAsync()
        {
            var response = new ApiResponse();
            try
            {
                var model = JsonConvert.DeserializeObject<SiteAWSProperty>(HttpContext.Request.Form["model"][0]);
                model.UserId = CurrentUser.UserId;

                var result = await _service.SaveAWSPropertySettingsAsync(model);

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

        #endregion 'Post'

        #region 'ViewComponent'
        [HttpGet]
        public IActionResult EmailSettingComponent()
        {
            return ViewComponent("EmailSettingComponent");
        }

        [HttpGet]
        public IActionResult PushNotificationSettingComponent()
        {
            return ViewComponent("PushNotificationSettingComponent");
        }

        [HttpGet]
        public IActionResult AWSPropertySettingComponent()
        {
            return ViewComponent("AWSPropertySettingComponent");
        }
        #endregion 'ViewComponent'
    }
}
