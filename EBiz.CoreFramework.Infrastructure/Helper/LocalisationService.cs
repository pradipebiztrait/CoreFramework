using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	[ScopedService]
	public class LocalisationService : ILocalisationService
	{
		private IConfiguration _configuration;
		private string _baseUrl;

		public LocalisationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string BaseUrl => string.IsNullOrEmpty(_baseUrl)
			? _baseUrl = _configuration.GetSection("SiteSettings")["BaseUrl"].ToString()
			: _baseUrl;

		public string SiteName => "Ebiz Core";
	}
}
