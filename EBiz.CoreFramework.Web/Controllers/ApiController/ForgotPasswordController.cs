using System;
using System.Linq;
using System.Threading.Tasks;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Infrastructure.Utility;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Helper;
using EBiz.CoreFramework.Service.Services;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
    [ApiController]
    public class ForgotPasswordController : ApiBaseController
    {
        #region 'Declair Variable'
        private readonly ApplicationDbContext _context;
        private readonly SiteSettings _siteSettings;
        public readonly IServiceProvider _serviceProvider;
        public readonly IEmailService _emailService;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ITokenService _tokenService;
        #endregion 'Declair Variable'

        #region 'Constructor'
        public ForgotPasswordController(ApplicationDbContext context
            , IServiceProvider serviceProvider
            , IEmailService emailService
            , IHostingEnvironment appEnvironment
			, ITokenService tokenService
			, IOptions<SiteSettings> siteSettings)
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _emailService = emailService;
            _appEnvironment = appEnvironment;
			_tokenService = tokenService;
			_siteSettings = siteSettings.Value;

		}
        #endregion 'Constructor'

        #region 'Post'
        [HttpPost("ForgotPassword")]
        public async Task<ApiResponse> ForgotPassword([FromBody]JObject req)
        {
			var _res = new ApiResponse();
            try
            {
                var email = req["Email"].ToObject<string>();
                if (string.IsNullOrEmpty(email))
                {
					_res.Status = false;
					_res.Message = "Please enter email address.";
					return _res;
                }

				var token = _tokenService.TokenGenerateByEmail(email);

				var resetLink = _siteSettings.BaseUrl + "/ResetPassword?token=" + token;
				//var resetLink = Url.Action("ResetPassword", "ForgotPassword", new { token = token }, protocol: HttpContext.Request.Scheme);

                var emailBody = _emailService.CreateEmailBody(resetLink, "ForgotPassword.html");
                _emailService.SendMailbyThread(email, "Reset Password", emailBody, null);

				_res.Status = true;
				_res.Message = "We have Email your password reset link.";
				return _res;
            }
            catch (Exception ex)
            {
				_res.Status = true;
				_res.Message = ex.Message.ToString();
				return _res;
            }
        }

		[HttpPost("ChangePassword")]
		public async Task<ApiResponse> ChangePassword([FromBody]JObject req)
		{
			var _apiRes = new ApiResponse();

			var oldpassword = req["OldPassword"].ToObject<string>();
			var password = req["Password"].ToObject<string>();
			var conPassword = req["ConfirmPassword"].ToObject<string>();

			bool IsMatch = String.Equals(password, conPassword);

			if (!IsMatch)
			{
				_apiRes.Status = false;
				_apiRes.Message = "Password and Confirm Password doesn't match.";
				return _apiRes;
			}

			var _user = _context.users.Where(t => t.Password == new Security().Encrypt(oldpassword) && t.Token == ApiToken).FirstOrDefault();

			if(_user == null)
			{
				_apiRes.Status = false;
				_apiRes.Message = "Unauthorized user.";
				return _apiRes;
			}

			_user.Password = new Security().Encrypt(password.Trim());

			_context.users.Update(_user);
			_context.SaveChanges();

			_apiRes.Status = true;
			_apiRes.Message = "Password has been changed successfully.";

			return _apiRes;
		}
		#endregion 'Post'
	}
}