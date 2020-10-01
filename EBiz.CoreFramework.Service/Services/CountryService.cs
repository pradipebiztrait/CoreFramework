using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;

		public CountryService(ICountryRepository countryRepository)
		{
			_countryRepository = countryRepository;
		}

		public async Task<ApiResponse> GetAllCountries()
		{
			var response = new ApiResponse();
			var data = await _countryRepository.GetAllCountries();
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}
	}
}
