using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface ICityRepository : IScopedService
	{
		Task<ApiResponse> GetAllCities(Int64 id);
	}
}
