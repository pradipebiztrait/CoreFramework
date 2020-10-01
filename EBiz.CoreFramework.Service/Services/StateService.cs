using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class StateService : IStateService
	{
		private readonly IStateRepository _stateRepository;

		public StateService(IStateRepository stateRepository)
		{
			_stateRepository = stateRepository;
		}

		public async Task<ApiResponse> GetAllStates(Int64 id)
		{
			return await _stateRepository.GetAllStates(id);
		}
	}
}
