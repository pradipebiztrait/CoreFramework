using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	public interface ICountryService : IScopedService
	{
		Task<ApiResponse> GetAllCountries();
	}
}
