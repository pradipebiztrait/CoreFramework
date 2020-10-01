using EBiz.CoreFramework.DataAccess.DbContext;
using System;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public interface IUnitOfWorkRepository : IDisposable
    {
        ApplicationDbContext Context { get; }
        void Commit();
    }
}
