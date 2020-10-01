using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System;
using EBiz.CoreFramework.DataAccess;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class CityRepository : ICityRepository
    {
        public readonly IDapperService _dapperService;

        public CityRepository(IDapperService dapperService)
        {
			_dapperService = dapperService;
		}

		#region 'APIs'
		public async Task<ApiResponse> GetAllCities(Int64 id)
		{
			var _apiRes = new ApiResponse();

			var dbPara = new DynamicParameters();
			dbPara.Add("@uStateId", id, DbType.Int64);
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			_apiRes.Data = Task.FromResult(await _dapperService.GetAllAsync<City>("API_GetAllCities", dbPara, CommandType.StoredProcedure)).Result;

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiRes;
		}
		#endregion 'APIs'
	}
}