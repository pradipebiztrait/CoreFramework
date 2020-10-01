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
	public class StateController : ApiBaseController
	{
		#region 'Declair Variable'
		public readonly IStateService _stateService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public StateController(IStateService stateService)
		{
			_stateService = stateService;
		}
		#endregion 'Constructor'

		#region 'POST'
		[HttpPost("GetAllState")]
		public async Task<ApiResponse> GetAllState([FromBody]JObject req)
		{
			var response = new ApiResponse();
			var id = req["CountryId"].ToObject<Int64>();

			var data = await _stateService.GetAllStates(id);

			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}
		#endregion 'POST'
	}
}