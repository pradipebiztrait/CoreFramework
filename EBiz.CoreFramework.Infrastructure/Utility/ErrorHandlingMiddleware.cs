using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Utility
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		public Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			var _apiRes = new ApiResponse();
			_apiRes.Message = ex.Message;
			_apiRes.Status = false;

			var result = JsonConvert.SerializeObject(_apiRes);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return context.Response.WriteAsync(result);
		}
	}
}
