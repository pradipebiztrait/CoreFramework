using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public interface ICompanyRepository : IScopedService
    {
        Task<ApiResponse> GetAsync();
        Task<ApiResponse> SaveAsync(Company model);
        void UpdateLogoPath(Int64 id, string path);
    }
}
