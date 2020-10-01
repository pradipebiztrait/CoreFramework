using EBiz.CoreFramework.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public Repository(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
            _unitOfWork.Commit();
        }

        public void AddRange(List<T> entity)
        {
            _unitOfWork.Context.Set<T>().AddRangeAsync(entity);
            _unitOfWork.Commit();
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public void DeleteById(Int64 id, string field)
        {
            var predicate = IsMatchedExpression(id, field);

            T existing = _unitOfWork.Context.Set<T>().Where(predicate).FirstOrDefault();
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
            _unitOfWork.Commit();
        }

        public void DeleteListById(Int64 id, string field)
        {
            var predicate = IsMatchedExpression(id, field);

            List<T> existing = _unitOfWork.Context.Set<T>().Where(predicate).ToList();
            if (existing != null) _unitOfWork.Context.Set<T>().RemoveRange(existing);
            _unitOfWork.Commit();
        }

        public void DeleteListByTripId(Int64 id, string field,Int64 TableTypes)
        {
            var predicate1 = IsMatchedExpression(id, field);
            var predicate2 = IsMatchedExpression(Convert.ToInt32(TableTypes), "TableType");

            List<T> existing = _unitOfWork.Context.Set<T>().Where(predicate1).Where(predicate2).ToList();
            if (existing != null) _unitOfWork.Context.Set<T>().RemoveRange(existing);
            _unitOfWork.Commit();
        }

        public List<T> GetAll() => _unitOfWork.Context.Set<T>().ToList();

        public async Task<List<T>> GetAllAsync() => await _unitOfWork.Context.Set<T>().ToListAsync();

        public List<T> GetAllBySP(string spName) => _unitOfWork.Context.Set<T>().FromSql(spName).ToList();

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            return _unitOfWork.Context.Set<T>().Where(predicate).AsEnumerable<T>();
        }

        public IEnumerable<T> GetById(object id,string field)
        {
            var predicate = IsMatchedExpression( id,field);

            return _unitOfWork.Context.Set<T>().Where(predicate);
        }

        public T GetByFielsAsync(object id, string field)
        {
            var predicate = IsMatchedExpression(id, field);

            return _unitOfWork.Context.Set<T>().Where(predicate).FirstOrDefault();
        }

        public bool IsMatchedRole(RoleType roleType)
        {
            //var roleRep = new Repository<AspNetRole>(_unitOfWork);
            //var userRoleRep = new Repository<AspNetUserRole>(_unitOfWork);
            //var studentToStandard = roleRep.GetAll().Join(userRoleRep.GetAll(),
            //                      r => r.Id,
            //                      ur => ur.RoleId,
            //                      (role, userrole) => new { AspNetRole = role, AspNetUserRole = userrole }).ToList();

            var predicate = IsMatchedRole(roleType, "Name");
            return _unitOfWork.Context.Set<T>().Any(predicate);
        }

        public void Update(T entity)
        {
            try
            {
                _unitOfWork.Context.Set<T>().Attach(entity);
                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public Expression<Func<T, bool>> IsMatchedExpression(object id, string field)
        {
            var parameterExpression = Expression.Parameter(typeof(T));
            var propertyOrField = Expression.PropertyOrField(parameterExpression, field);
            var binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(id));
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        }

        public Expression<Func<T, bool>> IsMatchedRole(RoleType role, string field)
        {
            var parameterExpression = Expression.Parameter(typeof(T));
            var propertyOrField = Expression.PropertyOrField(parameterExpression, field);
            var binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(role));
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        }
    }
}
