using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
    public interface ICompanyService : IScopedService
    {
        Task<ApiResponse> GetAsync();
        Task<ApiResponse> SaveAsync(Company model);
        void UpdateLogoPath(Int64 id, string path);
    }
}
