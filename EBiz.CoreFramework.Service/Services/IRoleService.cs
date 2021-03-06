﻿using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	public interface IRoleService : IScopedService
	{
		Task<ApiResponse> SaveAsync(Roles model);
		Task<ApiResponse> GetAsync(Int64 id);
        Task<ApiResponse> GetAllByFilterAsync(FilterRequest request);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
        Task<ApiResponse> GetUserPermissionAsync(Int64 id);
        Task<ApiResponse> SaveRolePermissionAsync(List<RolePermission> model);
        Task<RolePermission> PermissionAsync(Int64 id, string menu);
    }
}
