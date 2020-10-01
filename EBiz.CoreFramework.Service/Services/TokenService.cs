using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Helper;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;

namespace EBiz.CoreFramework.Service.Services
{
    [ScopedService]
	public class TokenService : ITokenService
	{
		private readonly SiteSettings _siteSettings;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly IUserService _userService;
		public readonly IServiceProvider _serviceProvider;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public TokenService(IOptions<SiteSettings> siteSettings
			, IUserService userService
			, IJwtTokenService jwtTokenService
			, IServiceProvider serviceProvider
			, SignInManager<ApplicationUser> signInManager
			, ApplicationDbContext context)
		{
			_siteSettings = siteSettings.Value;
			_userService = userService;
			_jwtTokenService = jwtTokenService;
			_serviceProvider = serviceProvider;
			_signInManager = signInManager;
			_context = context;
		}

		public async Task<Tuple<string, User, ApiResponse>> TokenGenerate(string emailId, string password)
		{
			var result = await _userService.GetUserByEmailAndPassword(emailId.Trim(), new Security().Encrypt(password.Trim()));

			//Authenticate User, Check if it’s a registered user in Database 
			if (!result.Status)
				return new Tuple<string, User, ApiResponse>(null, result.Data, result);

			if (password == new Security().Decrypt(result.Data.Password))
			{
				var token = _jwtTokenService.GenerateToken(result.Data.EmailAddress, Guid.NewGuid().ToString());

				result.Data.Password = null;
				return new Tuple<string, User, ApiResponse>(token, result.Data, result);
			}
			else
			{
				return new Tuple<string, User, ApiResponse>(null, result.Data, result);
			}
		}

		public string TokenGenerateByEmail(string email)
		{
			try
			{
				var result = _userService.GetUserByEmail(email.Trim());

				if (result != null)
				{
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub,email),
                    new Claim(JwtRegisteredClaimNames.Jti,Convert.ToString(result.Result.Data.UserId)),
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
				else
				{
					return "0";
				}

			}
			catch (Exception)
			{
				return "0";
			}
		}

		public string TokenGenerateSignUp(string email, Int64 userId)
		{
			try
			{
				if (!string.IsNullOrEmpty(email) && userId > 0)
				{
					var token = _jwtTokenService.GenerateToken(email, Convert.ToString(userId));
					var claims = new[]
					{
					new Claim(JwtRegisteredClaimNames.Sub,email),
					new Claim(JwtRegisteredClaimNames.Jti,Convert.ToString(userId)),
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
				else
				{
					return "0";
				}

			}
			catch (Exception)
			{
				return "0";
			}
		}

		public async Task<Tuple<string, User, ApiResponse>> TokenGenerateWithUDID(AccessDevice accessDevice)
		{

			var result = await _userService.GetAccessDevice(accessDevice);

			if (result == null)
			{
				var dtRes = new ApiResponse();
				dtRes.Message = "Email not registred!";
				return new Tuple<string, User, ApiResponse>(null, null, dtRes);
			}

			//Authenticate User, Check if it’s a registered user in Database 
			if (!result.Data.Item1.IsSuccess)
				return new Tuple<string, User, ApiResponse>(null, result.Data.Item2, result.Data.Item1);

			if (accessDevice.Password == new Security().Decrypt(result.Data.Item2.Password))
			{
				var JWToken = _jwtTokenService.GenerateToken(result.Data.Item2.EmailAddress, Guid.NewGuid().ToString());


				var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

				return new Tuple<string, User, ApiResponse>(token, result.Data.Item2, result.Data.Item1);
			}
			else
			{
				return new Tuple<string, User, ApiResponse>(null, result.Data.Item2, result.Data.Item1);
			}
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

		public string DecodeAdminJwtToken(string token)
		{
			try
			{
				var stream = token;
				var handler = new JwtSecurityTokenHandler();
				var jsonToken = handler.ReadToken(stream);
				var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

				return tokenS.Claims.First(claim => claim.Type == "jti").Value;
			}
			catch
			{
				return "0";
			}
		}

		//public bool IsValidToken(string token, Int64 userId)
		//{
		//	try
		//	{
		//		var exists = _context.users.Where(t => t.Token == token && t.UserId == userId).FirstOrDefault();

		//		if (exists != null)
		//		{
		//			return true;
		//		}
		//		else
		//		{
		//			return false;
		//		}
		//	}
		//	catch
		//	{
		//		return false;
		//	}
		//}
	}
}
