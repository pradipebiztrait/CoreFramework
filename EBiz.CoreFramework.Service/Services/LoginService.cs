using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class LoginService : ILoginService
	{
		private readonly ILoginRepository _loginRepository;

		public LoginService(ILoginRepository loginRepository)
		{
			_loginRepository = loginRepository;
		}
	}
}
