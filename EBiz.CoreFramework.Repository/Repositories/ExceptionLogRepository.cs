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
	public class ExceptionLogRepository : IExceptionLogRepository
    {
		private readonly IDapperService _dapperService;
		private readonly ApplicationDbContext _context;

		public ExceptionLogRepository(IDapperService dapperService, ApplicationDbContext context)
		{
			_dapperService = dapperService;
            _context = context;

        }

        public async Task<ApiResponse> GetAsync(Int64 id)
        {
            return new ApiResponse()
            {
                Data = await _context.exception_log.Where(t => t.exception_log_id == id).FirstOrDefaultAsync(),
                Message = "Success",
                Status = true,
            };
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

            using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetAllExceptionLogByFilters", _param, CommandType.StoredProcedure)).Result)
            {
                _apiResponse.Data = gridReader.Read<ExceptionLog>().ToList();
                _apiResponse.TotalRecord = gridReader.Read<Int32>().FirstOrDefault();
            }

            _apiResponse.Message = _param.Get<string>("oMessage");
            _apiResponse.Status = _param.Get<Int16>("oStatus") == 1 ? true : false;

            return _apiResponse;
        }

		public async Task SaveAsync(ExceptionLog model)
		{
			await _context.exception_log.AddAsync(model);
			await _context.SaveChangesAsync();
		}
	}
}
