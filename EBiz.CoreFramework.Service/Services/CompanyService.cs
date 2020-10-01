using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> GetAsync() => await _repository.GetAsync();

        public async Task<ApiResponse> SaveAsync(Company model) => await _repository.SaveAsync(model);

        public void UpdateLogoPath(Int64 id, string path) => _repository.UpdateLogoPath(id, path);
    }
}
