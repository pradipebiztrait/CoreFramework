using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Dapper;
using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class MenuRepository : IMenuRepository
	{
		private readonly IDapperService _dapperService;
		private readonly ApplicationDbContext _context;

		public MenuRepository(IDapperService dapperService, ApplicationDbContext context)
		{
			_dapperService = dapperService;
            _context = context;

        }

        public async Task<ApiResponse> GetAsync(Int64 id)
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.menu.Where(t=>t.menu_id == id).FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> GetAllParentMenuAsync()
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.menu.Where(t => t.parent_menu_id == 0).ToListAsync();

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

            using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetAllMenuByFilters", _param, CommandType.StoredProcedure)).Result)
            {
                _apiResponse.Data = gridReader.Read<Menu>().ToList();
                _apiResponse.TotalRecord = gridReader.Read<Int32>().FirstOrDefault();
            }

            _apiResponse.Message = _param.Get<string>("oMessage");
            _apiResponse.Status = _param.Get<Int16>("oStatus") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> GetUserMenuAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();
            var _param = new DynamicParameters();

            _param.Add("@orole_id", id, DbType.Int64);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            var result = Task.FromResult(await _dapperService.GetAllAsync<Menu>("API_GetUserMenu", _param, CommandType.StoredProcedure)).Result;

            _apiResponse.Data = result;
            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> SaveAsync(Menu model)
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@omenu_id", model.menu_id, DbType.Int64);
			dbPara.Add("@oparent_menu_id", model.parent_menu_id, DbType.Int64);
			dbPara.Add("@oUserId", model.UserId, DbType.Int64);
			dbPara.Add("@omenu_title", model.menu_title, DbType.String);
			dbPara.Add("@omenu_url", model.menu_url, DbType.String);
			dbPara.Add("@omenu_icon", model.menu_icon, DbType.String);
			dbPara.Add("@osort_order", model.sort_order, DbType.Int32);
			dbPara.Add("@ois_active", model.is_active, DbType.Int32);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			await _dapperService.GetAsync<Pages>("API_SaveMenu", dbPara, CommandType.StoredProcedure);

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiRes;
		}

        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@oid", id, DbType.Int64);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = await _dapperService.GetAsync<Menu>("API_DeleteMenu", _param, CommandType.StoredProcedure);

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

            _apiResponse.Data = await _dapperService.GetAsync<Menu>("API_DeleteMultipleMenu", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }
    }
}
