using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Helper;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
    [ApiController]
    public class CmsController : ApiBaseController
    {
		#region 'Declair Variable'
		public readonly ICmsService _cmsService;
		public readonly IEmailService _emailService;
		private readonly IHostingEnvironment _environment;
		private readonly EmailSettings _emailSettings;
		private readonly ApplicationDbContext _context;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public CmsController(
			ICmsService cmsService
			, IEmailService emailService
			, IHostingEnvironment environment
			, IOptions<EmailSettings> emailSettings
			, ApplicationDbContext context)
		{
			_cmsService = cmsService;
			_emailService = emailService;
			_environment = environment;
			_emailSettings = emailSettings.Value;
			_context = context;
		}
		#endregion 'Constructor'

		#region 'Post'
		[Route("Page/{page}")]
		public async Task<ApiResponse> Page(string page)
		{
			var response = new ApiResponse();

			var result = await _cmsService.GetPageAsync(page);

			if(result.Data != null)
			{
				response.Data = result.Data.PageDescription;
			}
			else
			{
				response.Data = "<h1>Page Not Found</p>";
			}

			response.Message = result.Message;
			response.Status = result.Status;

			return response;
		}

		[HttpPost("ContactUs")]
		public async Task<ApiResponse> ContactUs([FromBody]JObject req)
		{
			var response = new ApiResponse();
			var model = new ContactUs();

			model.UserId = req["UserId"].ToObject<Int64>();
			model.FullName = req["FullName"].ToObject<string>();
			model.Email = req["Email"].ToObject<string>();
			model.PhoneNumber = req["PhoneNumber"].ToObject<string>();
			model.Message = req["Message"].ToObject<string>();

			if(_context.users.Where(t=>t.Token == ApiToken && t.UserId == model.UserId).FirstOrDefault() != null)
			{
				var emailBody = CreateEmailBody(model);
				_emailService.SendMailbyThread(_emailSettings.Sender, "Contact Us by User", emailBody, null);

				response.Message = "Your detail has been sent to administrator successfully.";
				response.Status = true;
			}
			else
			{
				response.Message = "Unauthorized user.";
				response.Status = false;
			}

			return response;
		}

		public string CreateEmailBody(ContactUs model)
		{

			string body = string.Empty;

			var templatePath = _environment.WebRootPath
							+ Path.DirectorySeparatorChar.ToString()
							+ "Templates"
							+ Path.DirectorySeparatorChar.ToString()
							+ "ContactUs.html";

			using (StreamReader reader = new StreamReader(templatePath))
			{
				body = reader.ReadToEnd();
			}
			body = body.Replace("{FullName}", model.FullName)
				.Replace("{Email}", model.Email)
				.Replace("{Message}", model.Message)
				.Replace("{PhoneNumber}", model.PhoneNumber);

			return body;
		}
		#endregion 'Post'

	}
}