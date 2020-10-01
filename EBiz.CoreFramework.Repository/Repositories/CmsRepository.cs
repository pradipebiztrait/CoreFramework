using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Dapper;
using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class CmsRepository : ICmsRepository
	{
		private readonly IDapperService _dapperService;

		public CmsRepository(IDapperService dapperService)
		{
			_dapperService = dapperService;
		}

		public async Task<ApiResponse> GetAsync(Int64 id)
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@oPageId", id, DbType.Int64);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			_apiRes.Data = await _dapperService.GetAsync<Pages>("API_GetCMS", dbPara, CommandType.StoredProcedure);

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiRes;
		}

		public async Task<ApiResponse> GetPageAsync(string url)
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@oPageUrl", "/" + url, DbType.String);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			_apiRes.Data = await _dapperService.GetAsync<Pages>("API_GetCMSByPageUrl", dbPara, CommandType.StoredProcedure);

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiRes;
		}

		public async Task<ApiResponse> GetAllAsync()
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			var data = await _dapperService.GetAllAsync<Pages>("API_GetAllCMS", dbPara, CommandType.StoredProcedure);

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;
			_apiRes.Data = data;

			return _apiRes;
		}

        public async Task<ApiResponse> GetAllPageByFilterAsync(FilterRequest request)
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

            using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetAllPageByFilters", _param, CommandType.StoredProcedure)).Result)
            {
                _apiResponse.Data = gridReader.Read<Pages>().ToList();
                _apiResponse.TotalRecord = gridReader.Read<Int32>().FirstOrDefault();
            }

            _apiResponse.Message = _param.Get<string>("oMessage");
            _apiResponse.Status = _param.Get<Int16>("oStatus") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> SaveAsync(Pages model)
		{
			var _apiRes = new ApiResponse();
			var dbPara = new DynamicParameters();

			dbPara.Add("@oPageId", model.PageId, DbType.Int64);
			dbPara.Add("@oUserId", model.UserId, DbType.Int64);
			dbPara.Add("@oPageTitle", model.PageTitle, DbType.String);
			dbPara.Add("@oPageUrl", model.PageUrl, DbType.String);
			dbPara.Add("@oPageDescription", model.PageDescription, DbType.String);
			dbPara.Add("@oIsActive", model.IsActive, DbType.Int32);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			await _dapperService.GetAsync<Pages>("API_SaveCMS", dbPara, CommandType.StoredProcedure);

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

            _apiResponse.Data = await _dapperService.GetAsync<Pages>("API_DeletePage", _param, CommandType.StoredProcedure);

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

            _apiResponse.Data = await _dapperService.GetAsync<Pages>("API_DeleteMultiplePage", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }
    }
}
