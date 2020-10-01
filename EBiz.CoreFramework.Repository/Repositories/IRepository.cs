using EBiz.CoreFramework.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        List<T> GetAllBySP(string spName);
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetById(object id, string field);
        void Add(T entity);
        void AddRange(List<T> entity);
        void Delete(T entity);
        void DeleteById(Int64 id, string field);
        void DeleteListById(Int64 id, string field);
        void DeleteListByTripId(Int64 id, string field,Int64 TableTypes);
        void Update(T entity);
        bool IsMatchedRole(RoleType role);
        T GetByFielsAsync(object id, string field);
    }
}
