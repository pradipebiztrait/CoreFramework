using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.ActionFilters
{
	public class CustomExceptionFilter : IAsyncExceptionFilter
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IModelMetadataProvider _modelMetadataProvider;
		private readonly ApplicationDbContext _context;
		private readonly SiteSettings _siteSettings;

		public CustomExceptionFilter(
			IHostingEnvironment hostingEnvironment,
			IModelMetadataProvider modelMetadataProvider,
			ApplicationDbContext context,
			IOptions<SiteSettings> siteSettings
			)
		{
			_hostingEnvironment = hostingEnvironment;
			_modelMetadataProvider = modelMetadataProvider;
			_context = context;
			_siteSettings = siteSettings.Value;
		}

		public async Task OnExceptionAsync(ExceptionContext context)
		{

			context.ExceptionHandled = true;

			var errorFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ErrorLogs");

			if (!System.IO.Directory.Exists(errorFolder))
			{
				System.IO.Directory.CreateDirectory(errorFolder);
			}

			string timestamp = DateTime.Now.ToString("d-MMM-yyyy", CultureInfo.InvariantCulture);
			var newFileName = $"Log_{timestamp}.txt";
			var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "ErrorLogs") + $@"\{newFileName}";

			var sysIP = string.Empty;

			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork) sysIP = ip.ToString();
			}

			StringBuilder expectionStringBuilder = new StringBuilder();
			expectionStringBuilder.AppendLine($"\nMessage            : " + context.Exception.Message);
			expectionStringBuilder.AppendLine("\nSource             : " + context.Exception.Source);
			expectionStringBuilder.AppendLine("\nStackTrace         :\n" + context.Exception.StackTrace);
			expectionStringBuilder.AppendLine("\nTargetSite         : " + context.Exception.TargetSite);

			var controllerName = context.RouteData.Values["controller"];
			var actionName = context.RouteData.Values["action"];
			var requestUrl = _siteSettings.BaseUrl + "/" + controllerName + "/" + actionName;
			string dateTime = DateTime.Now.ToString("d-MMM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

			if (!File.Exists(filepath))
			{
				using (var writer = File.CreateText(filepath))
				{
					
					await writer.WriteLineAsync("----------------------------------------------------------------------------------------------------------------------------------");
					await writer.WriteLineAsync($"DateTime           : {dateTime}");
					await writer.WriteLineAsync($"System IP Address  : {sysIP}");
					await writer.WriteLineAsync($"Request Url        : {requestUrl}");
					//await writer.WriteLineAsync($"ControllerName     : {controllerName}");
					//await writer.WriteLineAsync($"ActionName         : {actionName}");
					await writer.WriteLineAsync("\nException");
					await writer.WriteLineAsync(expectionStringBuilder.ToString());
				}
			}
			else
			{
				using (var writer = File.AppendText(filepath))
				{
					await writer.WriteLineAsync("----------------------------------------------------------------------------------------------------------------------------------");
					await writer.WriteLineAsync($"DateTime           : {dateTime}");
					await writer.WriteLineAsync($"System IP Address  : {sysIP}");
					await writer.WriteLineAsync($"Request Url        : {requestUrl}");
					await writer.WriteLineAsync("\nException");
					await writer.WriteLineAsync(expectionStringBuilder.ToString());
				}
			}

			var exceptionLog = new ExceptionLog()
			{
				exception_log_id = 0,
				message = context.Exception.Message,
				stack_trace = context.Exception.StackTrace,
				ip_address = sysIP,
				status_code = 500
			};

			//var _context = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
			//_context.exception_log.Add(exceptionLog);
			//_context.SaveChanges();

			var result = new ViewResult { ViewName = "Error" };
			result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
			result.ViewData.Add("Exception", context.Exception);

			context.Result = result;
			return;
		}
	}
}
