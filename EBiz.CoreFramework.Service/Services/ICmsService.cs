using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	public interface ICmsService : IScopedService
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
