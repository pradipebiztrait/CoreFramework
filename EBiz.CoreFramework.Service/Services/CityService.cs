using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class CityService : ICityService
	{
		private readonly ICityRepository _cityRepository;
		public CityService(ICityRepository cityRepository)
		{
			_cityRepository = cityRepository;
		}

		public async Task<ApiResponse> GetAllCities(Int64 id)
		{
			return await _cityRepository.GetAllCities(id);
		}
	}
}
