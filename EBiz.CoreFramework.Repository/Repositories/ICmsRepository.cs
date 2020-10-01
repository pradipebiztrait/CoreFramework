using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface ICmsRepository : IScopedService
	{
		Task<ApiResponse> SaveAsync(Pages model);
		Task<ApiResponse> GetAsync(Int64 id);
		Task<ApiResponse> GetAllAsync();
		Task<ApiResponse> GetPageAsync(string url);
        Task<ApiResponse> GetAllPageByFilterAsync(FilterRequest request);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
    }
}
