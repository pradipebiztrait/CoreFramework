using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Dapper;
using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class RoleRepository : IRoleRepository
    {
		private readonly IDapperService _dapperService;
		private readonly ApplicationDbContext _context;

		public RoleRepository(IDapperService dapperService, ApplicationDbContext context)
		{
			_dapperService = dapperService;
            _context = context;

        }

        public async Task<ApiResponse> GetAsync(Int64 id)
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.roles.Where(t=>t.role_id == id).FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> GetAllByFilterAsync(FilterRequest request)
        {
            var _apiResponse = new ApiResponse();
            var _param = new DynamicParameters();

            _param.Add("@oSkip", request.Skip, DbType.Int32);
            _param.Add("@oTake", request.Take, DbType.Int32);
            _param.Add("@oSearchText", request.SearchText, DbType.String);
            _param.Add("@oSortCol", request.SortCol, DbType.String);
            _param.Add("@oSortDir", request.SortDir, DbType.String);
            _param.Add("@oMessage", null, DbType.String, ParameterDirection.Output);
            _param.Add("@oStatus", 0, DbType.Int16, ParameterDirection.Output);

            using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetAllRoleByFilters", _param, CommandType.StoredProcedure)).Result)
            {
                _apiResponse.Data = gridReader.Read<Roles>().ToList();
                _apiResponse.TotalRecord = gridReader.Read<Int32>().FirstOrDefault();
            }

            _apiResponse.Message = _param.Get<string>("oMessage");
            _apiResponse.Status = _param.Get<Int16>("oStatus") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> GetUserPermissionAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();
            var _param = new DynamicParameters();

            _param.Add("@orole_id", id, DbType.Int32);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            var result = await _dapperService.GetAllAsync<RolePermission>("API_GetRolePermissions", _param, CommandType.StoredProcedure);

            _apiResponse.Data = result;
            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> GetPermissionAsync(Int64 id, string menu)
        {
            var _apiResponse = new ApiResponse();
            var _param = new DynamicParameters();

            _param.Add("@orole_id", id, DbType.Int32);
            _param.Add("@omenu_url", menu, DbType.String);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            var result = await _dapperService.GetAsync<RolePermission>("API_GetPermission", _param, CommandType.StoredProcedure);

            _apiResponse.Data = result;
            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> SaveAsync(Roles model)
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@orole_id", model.role_id, DbType.Int64);
			dbPara.Add("@oUserId", model.UserId, DbType.Int64);
			dbPara.Add("@orole_name", model.role_name, DbType.String);
			dbPara.Add("@onormalized_role", model.normalized_role, DbType.String);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			await _dapperService.GetAsync<Roles>("API_SaveRole", dbPara, CommandType.StoredProcedure);

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiRes;
		}

        public async Task<ApiResponse> SaveRolePermissionAsync(List<RolePermission> model)
        {
            var _apiRes = new ApiResponse();

            if(model.Select(t=>t.role_id).FirstOrDefault() == 0)
            {
                await _context.role_permissions.AddRangeAsync(model);
                await _context.SaveChangesAsync();
                _apiRes.Message = "Role Permission has been Added successfully.";
                _apiRes.Status = true;
            }
            if (model.Select(t => t.role_id).FirstOrDefault() > 0)
            {
                var oldPermissions = _context.role_permissions.Where(t => t.role_id == model.Select(x => x.role_id).FirstOrDefault()).ToList();
                _context.role_permissions.RemoveRange(oldPermissions);
                await _context.SaveChangesAsync();

                await _context.role_permissions.AddRangeAsync(model);
                await _context.SaveChangesAsync();
                _apiRes.Message = "Role Permission has been updated successfully.";
                _apiRes.Status = true;
            }


            return _apiRes;
        }

        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@oid", id, DbType.Int64);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = await _dapperService.GetAsync<Roles>("API_DeleteRole", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@oids", ids, DbType.String);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = await _dapperService.GetAsync<Roles>("API_DeleteMultipleRole", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }
    }
}
