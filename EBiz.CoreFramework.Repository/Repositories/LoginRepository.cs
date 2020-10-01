using EBiz.CoreFramework.DataAccess;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class LoginRepository : ILoginRepository
    {
        public readonly IDapperService _dapperService;

        public LoginRepository(IDapperService dapperService)
        {
			_dapperService = dapperService;
		}

		#region 'APIs'
		
		#endregion 'APIs'
	}
}