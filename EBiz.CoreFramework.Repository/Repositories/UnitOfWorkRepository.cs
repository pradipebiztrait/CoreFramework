using EBiz.CoreFramework.DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
	{
        public ApplicationDbContext Context { get; }

        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

        }
    }
}
