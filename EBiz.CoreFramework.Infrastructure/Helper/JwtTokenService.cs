using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	[ScopedService]
	public class JwtTokenService : IJwtTokenService
	{
		private readonly SiteSettings _siteSettings;
		public JwtTokenService(IOptions<SiteSettings> siteSettings)
		{
			_siteSettings = siteSettings.Value;
		}

		public string GenerateToken(string _sub, string _jti)
		{
			var claims = new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub,_sub),
					new Claim(JwtRegisteredClaimNames.Jti,_jti),
				};

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSettings.TokenString));

			var JWToken = new JwtSecurityToken(
					issuer: _siteSettings.BaseUrl,
					audience: _siteSettings.BaseUrl,
					expires: DateTime.UtcNow.AddHours(1),
					claims: claims,
					signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
				);

			return new JwtSecurityTokenHandler().WriteToken(JWToken);
		}

		public string DecodeJwtToken(string token)
		{
			try
			{
				var stream = token;
				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(stream);
				var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

				return tokenS.Claims.First(claim => claim.Type == "id").Value;
			}
			catch
			{
				return "0";
			}
		}
	}
}
