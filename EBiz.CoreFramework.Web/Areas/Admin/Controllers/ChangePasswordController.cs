using EBiz.CoreFramework.Web.Controllers.AdminControllers;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using EBiz.CoreFramework.Service.Services;

namespace EBiz.CoreFramework.Web.Areas.Admin.Controllers
{
	public class ChangePasswordController : BaseController
    {
        #region 'Declair Variable'
        public readonly IUserService _userService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public ChangePasswordController(IUserService userService)
		{
			_userService = userService;
		}
		#endregion 'Constructor'

		#region 'Get'
		public IActionResult Index()
        {
            var model = new ChangePassword()
            {
                UserName = CurrentUser.EmailAddress
            };

            return View(model);
        }

		#endregion 'Get'

		#region 'Post'
		[HttpPost("SubmitChangesPassword")]
		[Route("/Admin/SubmitChangesPassword")]
		public async Task<ApiResponse> SubmitChangesPassword()
		{
			var model = JsonConvert.DeserializeObject<ChangePassword>(HttpContext.Request.Form["model"][0]);

			return await _userService.AdminChangePasswordAsync(model);
		}
		#endregion 'Post'
	}
}