using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	public interface ILocalisationService : IScopedService
	{
		string BaseUrl { get; }
		string SiteName { get; }
	}
}
