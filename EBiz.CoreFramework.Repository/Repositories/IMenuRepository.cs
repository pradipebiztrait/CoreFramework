using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IMenuRepository : IScopedService
	{
		Task<ApiResponse> SaveAsync(Menu model);
		Task<ApiResponse> GetAsync(Int64 id);
		Task<ApiResponse> GetAllParentMenuAsync();
        Task<ApiResponse> GetAllByFilterAsync(FilterRequest request);
        Task<ApiResponse> GetUserMenuAsync(Int64 id);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
    }
}
