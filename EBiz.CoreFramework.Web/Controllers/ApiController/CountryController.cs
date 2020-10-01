using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EBiz.CoreFramework.Web.Utility;
using EBiz.CoreFramework.Service.Services;
using EBiz.CoreFramework.DataAccess.Models;

namespace EBiz.CoreFramework.Web.Controllers.ApiController
{
	[Route("api")]
	[ApiController]
	public class CountryController : ApiBaseController
	{
		#region 'Declair Variable'
		public readonly ICountryService _countryService;
		#endregion 'Declair Variable'

		#region 'Constructor'
		public CountryController(ICountryService countryService)
		{
			_countryService = countryService;
		}
		#endregion 'Constructor'

		#region 'POST'
		[HttpPost("GetAllCounty")]
		public async Task<ApiResponse> GetAllCounty()
		{
			var response = new ApiResponse();

			var data = await _countryService.GetAllCountries();

			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}
		#endregion 'POST'
	}
}