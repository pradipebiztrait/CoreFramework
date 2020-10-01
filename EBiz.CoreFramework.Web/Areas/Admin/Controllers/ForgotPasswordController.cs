using System;
using System.IO;
using System.Linq;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Helper;

namespace EBiz.CoreFramework.Web.AdminControllers
{
	[Area("Admin")]
	[Route("Admin")]
	public class ForgotPasswordController : Controller
	{
		#region 'Declair Variable'
		private readonly ApplicationDbContext _context;
		public readonly IServiceProvider _serviceProvider;
		public readonly IEmailService _emailService;
		public readonly IUserService _userService;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly ITokenService _tokenService;
		private readonly SiteSettings _siteSettings;

		#endregion 'Declair Variable'

		#region 'Constructor'
		public ForgotPasswordController(ApplicationDbContext context
			, IOptions<SiteSettings> siteSettings
			, IServiceProvider serviceProvider
			, IEmailService emailService
			, ITokenService tokenService
			, IHostingEnvironment appEnvironment
			, IUserService userService)
		{
			_serviceProvider = serviceProvider;
			_context = context;
			_emailService = emailService;
			_tokenService = tokenService;
			_appEnvironment = appEnvironment;
			_userService = userService;
			_siteSettings = siteSettings.Value;

		}
		#endregion 'Constructor'

		#region 'Get'
		[Route("ForgotPassword")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("AdminResetPassword")]
		public IActionResult AdminResetPassword(string token)
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
						Token = usr.Token
					};

					return View(model);
				}
			}

			TempData["ErrorMessage"] = "Sorry! Your are access unauthorized token.";

			return View("Notification");
		}

		#endregion 'Get'

		#region 'Post'
		[HttpPost("AdminResetPassword")]
		public IActionResult AdminResetPassword(ResetPassword model)
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

						return RedirectToAction("Index", "Login", new { area = "Admin" });
					}
					else
					{
						return View("AdminResetPassword", model);
					}
				}
				else
				{
					return View("AdminResetPassword", model);
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message.ToString();

				return RedirectToAction("Index", "Login", new { area = "Admin" });
			}
		}

		[HttpPost("SendPasswordResetLink")]
		public IActionResult SendPasswordResetLink(ForgotPassword model)
		{
			try
			{
				if (String.IsNullOrEmpty(model.EmailAddress))
				{
					TempData["ErrorMessage"] = "Please enter email address.";
					return RedirectToAction("Index", "ForgotPassword");
				}
				var user = _userService.GetUserByEmail(model.EmailAddress);

				if (user.Result.Data == null)
				{
					TempData["ErrorMessage"] = "Please enter valid email address.";
					return RedirectToAction("Index", "ForgotPassword");
				}

				if (user.Result.Data.EmailAddress == null)
				{
					TempData["ErrorMessage"] = "Please enter valid email address.";
					return RedirectToAction("Index", "ForgotPassword");
				}

				if (user.Result.Data.IsAdmin != 1)
				{
					TempData["ErrorMessage"] = "Unauthorized admin email address.";
					return RedirectToAction("Index", "ForgotPassword");
				}

				var token = _tokenService.TokenGenerateByEmail(model.EmailAddress);

				var resetLink = Url.Action("AdminResetPassword", "ForgotPassword", new { token = token }, protocol: HttpContext.Request.Scheme);

				//var isTokenUpdate = _userRepository.UpdateUserTokenAsync(user.Result.Result.user_id, token);

				if (token != "0")
				{
					var emailBody = CreateEmailBody(resetLink);

					_emailService.SendEmailAsync(model.EmailAddress, "Reset password.", emailBody, null);

					TempData["SuccessMessage"] = "Password reset link has been sent to your email address!";
				}
				else
				{
					TempData["ErrorMessage"] = "Something went to wrong Please try again.";
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = ex.Message.ToString();
			}


			return RedirectToAction("Index", "ForgotPassword", new { area = "Admin" });
			//return View("Notification");

		}
		#endregion 'Post'

		#region 'Functions'
		private string CreateEmailBody(string resetLink)
		{

			string body = string.Empty;
			//using streamreader for reading my htmltemplate   

			var templatePath = _appEnvironment.WebRootPath
							+ Path.DirectorySeparatorChar.ToString()
							+ "Templates"
							+ Path.DirectorySeparatorChar.ToString()
							+ "ForgotPassword.html";

			using (StreamReader reader = new StreamReader(templatePath))
			{
				body = reader.ReadToEnd();
			}

			body = body.Replace("{resetLink}", resetLink);

			return body;

		}

		private bool ValidateToken(string authToken)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var validationParameters = GetValidationParameters();

				SecurityToken validatedToken;
				IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private TokenValidationParameters GetValidationParameters()
		{
			return new TokenValidationParameters()
			{
				ValidateLifetime = false, // Because there is no expiration in the generated token
				ValidateAudience = false, // Because there is no audiance in the generated token
				ValidateIssuer = false,   // Because there is no issuer in the generated token
				ValidIssuer = "https://localhost:44383/",
				ValidAudience = "https://localhost:44383/",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.TokenString)) // The same key as the one that generate the token
			};
		}

		private string DecodeJwtToken(string token)
		{
			try
			{
				var stream = token;
				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(stream);
				var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

				return tokenS.Claims.First(claim => claim.Type == "id").Value;
			}
			catch
			{
				return "0";
			}
		}

		#endregion 'Functions'
	}
}