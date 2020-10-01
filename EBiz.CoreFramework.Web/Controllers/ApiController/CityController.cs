using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using EBiz.CoreFramework.Web.Utility;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.Models;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
	[ApiController]
	public class CityController : ApiBaseController
	{
		#region 'Declair Variable'
		public readonly ICityService _cityService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public CityController(ICityService cityService)
		{
			_cityService = cityService;
		}
		#endregion 'Constructor'

		#region 'POST'
		[HttpPost("GetAllCity")]
		public async Task<ApiResponse> GetAllCity([FromBody]JObject req)
		{
			var response = new ApiResponse();
			var id = req["StateId"].ToObject<Int64>();
			var data = await _cityService.GetAllCities(id);

			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}
		#endregion 'POST'
	}
}