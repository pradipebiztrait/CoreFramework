using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EBiz.CoreFramework.DataAccess
{
	public interface IDapperService : IScopedService
	{
		T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T SaveAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		Task<GridReader> GetMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
	}
}
