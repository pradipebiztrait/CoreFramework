using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Infrastructure.Utility;
using System;
using EBiz.CoreFramework.DataAccess.Models;
using System.Linq;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Controllers
{
	public class HomeController : Controller
    {
        #region 'Declair Variable'
		private readonly ITokenService _tokenService;
		private readonly IUserService _userService;
		private readonly ApplicationDbContext _context;
        private IMemoryCache _cache;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public HomeController(ITokenService tokenService
			, IUserService userService
			, ApplicationDbContext context
            , IMemoryCache memoryCache)
        {
			_tokenService = tokenService;
			_userService = userService;
			_context = context;
            _cache = memoryCache;
        }
        #endregion 'Constructor'

        #region 'Get'
        public IActionResult Index()
        {
			//return Redirect("/Admin/Login");
			return View();
        }

		[HttpGet("Activation")]
		public IActionResult Activation(string token)
		{
			var user_id = _tokenService.DecodeJwtToken(token);

			if (!string.IsNullOrEmpty(user_id))
			{
				if (Convert.ToInt32(user_id) == 0)
				{
					TempData["ErrorMessage"] = "Unauthorized token has been access.";

					return View();
				}

				var userDetail = _userService.ActiveUser(Convert.ToInt32(user_id));

				if (userDetail.Result.Status)
				{
					TempData["SuccessMessage"] = userDetail.Result.Message;

				}
				else
				{
					TempData["ErrorMessage"] = userDetail.Result.Message;
				}
			}

			return View();
		}

		[HttpGet("ResetPassword")]
		public IActionResult ResetPassword(string token)
		{
			var user_id = _tokenService.DecodeAdminJwtToken(token);

			if (!string.IsNullOrEmpty(user_id))
			{
				var userDetail = _userService.GetById(Convert.ToInt32(user_id));

				if (userDetail.Result != null)
				{
					var usr = userDetail.Result;
					var model = new ResetPassword()
					{
						UserName = usr.EmailAddress,
						UserId = usr.UserId,
						Token = token
					};
					return View(model);
				}
			}

			TempData["ErrorMessage"] = "Sorry! Your are access unauthorized token.";
			return View("Activation");
		}
		#endregion 'Get'

		#region 'Post'
		[HttpPost("ResetPassword")]
		public IActionResult ResetPassword(ResetPassword model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (model != null)
					{
						var _user = _context.users.Where(t => t.EmailAddress == model.UserName).FirstOrDefault();

						_user.Password = new Security().Encrypt(model.Password.Trim());

						_context.users.Update(_user);
						_context.SaveChanges();

						TempData["SuccessMessage"] = "Congratulations! Your password has been reset successfully.";

						return View("Activation");
					}
					else
					{
						return View("ResetPassword", model);
					}
				}
				else
				{
					return View("ResetPassword", model);
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message.ToString();
				return View("Activation");
			}
		}
        #endregion
    }
}
