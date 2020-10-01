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
	public class CmsService : ICmsService
	{
		private readonly ICmsRepository _cmsRepository;

		public CmsService(ICmsRepository cmsRepository)
		{
			_cmsRepository = cmsRepository;
		}

		public async Task<ApiResponse> GetAsync(Int64 id) => await _cmsRepository.GetAsync(id);

        public async Task<ApiResponse> GetPageAsync(string url) => await _cmsRepository.GetPageAsync(url);

        public async Task<ApiResponse> GetAllAsync() => await _cmsRepository.GetAllAsync();

        public async Task<ApiResponse> SaveAsync(Pages model) => await _cmsRepository.SaveAsync(model);

        public async Task<ApiResponse> GetAllPageByFilterAsync(FilterRequest request) => await _cmsRepository.GetAllPageByFilterAsync(request);

        public async Task<ApiResponse> DeleteAsync(Int64 id) => await _cmsRepository.DeleteAsync(id);

        public async Task<ApiResponse> DeleteMultipleAsync(string ids) => await _cmsRepository.DeleteMultipleAsync(ids);

    }
}
