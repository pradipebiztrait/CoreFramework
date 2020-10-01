using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface ICountryRepository : IScopedService
	{
		Task<ApiResponse> GetAllCountries();
	}
}
