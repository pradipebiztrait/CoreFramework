using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
    [ScopedService]
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _repository;

		public RoleService(IRoleRepository repository)
		{
            _repository = repository;
		}

		public async Task<ApiResponse> GetAsync(Int64 id) => await _repository.GetAsync(id);

        public async Task<ApiResponse> GetUserPermissionAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();

            var result = await _repository.GetUserPermissionAsync(id);

            foreach (var item in result.Data)
            {
                item.role_id = id;
            }

            _apiResponse.Data = result.Data;
            _apiResponse.Status = result.Status;
            _apiResponse.Message = result.Message;

            return _apiResponse;
        }

        public async Task<RolePermission> PermissionAsync(Int64 id, string menu)
        {
            var data = await _repository.GetPermissionAsync(id, menu);

            return (RolePermission)data.Data;
        }

        public async Task<ApiResponse> SaveAsync(Roles model)
        {
            model.normalized_role = model.role_name.Replace(" ", "").ToUpper();
            return await _repository.SaveAsync(model);
        }

        public async Task<ApiResponse> SaveRolePermissionAsync(List<RolePermission> model) => await _repository.SaveRolePermissionAsync(model);

        public async Task<ApiResponse> GetAllByFilterAsync(FilterRequest request) => await _repository.GetAllByFilterAsync(request);

        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _repository.DeleteAsync(id);

        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _repository.DeleteMultipleAsync(ids);

    }
}
