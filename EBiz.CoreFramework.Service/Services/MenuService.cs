using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class MenuService : IMenuService
	{
		private readonly IMenuRepository _repository;

		public MenuService(IMenuRepository repository)
		{
            _repository = repository;
		}

		public async Task<ApiResponse> GetAsync(Int64 id) => await _repository.GetAsync(id);

        public async Task<ApiResponse> GetUserMenuAsync(Int64 id) => await _repository.GetUserMenuAsync(id);

        public async Task<ApiResponse> GetAllParentMenuAsync() => await _repository.GetAllParentMenuAsync();

        public async Task<ApiResponse> SaveAsync(Menu model) => await _repository.SaveAsync(model);

        public async Task<ApiResponse> GetAllByFilterAsync(FilterRequest request) => await _repository.GetAllByFilterAsync(request);

        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _repository.DeleteAsync(id);

        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _repository.DeleteMultipleAsync(ids);

    }
}
