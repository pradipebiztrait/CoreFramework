using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	public interface IJwtTokenService : IScopedService
	{
		string GenerateToken(string _sub, string _jti);
		string DecodeJwtToken(string token);
	}
}
