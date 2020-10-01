using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class CountryRepository : ICountryRepository
    {
        public readonly IDapperService _dapperService;

        public CountryRepository(IDapperService dapperService)
        {
			_dapperService = dapperService;
		}

		#region 'APIs'
		public async Task<ApiResponse> GetAllCountries()
		{
			var _apiRes = new ApiResponse();

			var dbPara = new DynamicParameters();
			dbPara.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			dbPara.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			_apiRes.Data = Task.FromResult(await _dapperService.GetAllAsync<Country>("API_GetAllCountries", dbPara, CommandType.StoredProcedure)).Result;

			_apiRes.Message = dbPara.Get<string>("SuccessMsg");
			_apiRes.Status = dbPara.Get<Int16>("IsSuccess") == 1 ? true : false;
			return _apiRes;
		}
		#endregion 'APIs'
	}
}