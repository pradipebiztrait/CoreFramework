using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IRoleRepository : IScopedService
	{
		Task<ApiResponse> SaveAsync(Roles model);
		Task<ApiResponse> GetAsync(Int64 id);
        Task<ApiResponse> GetAllByFilterAsync(FilterRequest request);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
        Task<ApiResponse> GetUserPermissionAsync(Int64 id);
        Task<ApiResponse> SaveRolePermissionAsync(List<RolePermission> model);
        Task<ApiResponse> GetPermissionAsync(Int64 id, string menu);
    }
}
