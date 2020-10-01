using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EBiz.CoreFramework.DataAccess
{
	[ScopedService]
	public class DapperService : IDapperService
	{
		private readonly IConfiguration _config;
		public readonly MySqlConnection _database;
		public DapperService(IConfiguration config)
		{
			_config = config;
            _database = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
		}

		public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return _database.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
		}

		public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return await _database.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
		}

		public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return _database.Query<T>(sp, parms, commandType: commandType).ToList();
		}

		public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return await _database.QueryAsync<T>(sp, parms, commandType: commandType);
		}

		public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return _database.Execute(sp, parms, commandType: commandType);
		}

		public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			T result;
			try
			{
				if (_database.State == ConnectionState.Closed)
                    _database.Open();

				var tran = _database.BeginTransaction();
				try
				{
					result = _database.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_database.State == ConnectionState.Open)
                    _database.Close();
			}

			return result;
		}

		public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			T result;
			try
			{
				if (_database.State == ConnectionState.Closed)
                    _database.Open();

				var tran = _database.BeginTransaction();
				try
				{
					result = _database.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_database.State == ConnectionState.Open)
                    _database.Close();
			}

			return result;
		}

		//new method
		public T SaveAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			T result;
			try
			{
				if (_database.State == ConnectionState.Closed)
                    _database.Open();

				var tran = _database.BeginTransaction();
				try
				{
					result = _database.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_database.State == ConnectionState.Open)
                    _database.Close();
			}

			return result;
		}

		public async Task<GridReader> GetMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			return await _database.QueryMultipleAsync(sp, parms, commandType: commandType);
		}

		public void Dispose()
		{

		}
	}
}
