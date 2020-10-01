using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class DashboardRepository : IDashboardRepository
	{
		private readonly IDapperService _dapperService;

		public DashboardRepository(IDapperService dapperService)
		{
			_dapperService = dapperService;
		}
	}
}
