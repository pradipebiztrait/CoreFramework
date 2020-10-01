using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IExceptionLogRepository : IScopedService
	{
		Task<ApiResponse> GetAsync(Int64 id);
        Task<ApiResponse> GetAllByFilterAsync(FilterRequest request);
		Task SaveAsync(ExceptionLog model);

	}
}
