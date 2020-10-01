using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	public interface ITokenService : IScopedService
	{
		Task<Tuple<string, User, ApiResponse>> TokenGenerate(string emailId, string password);
		string TokenGenerateByEmail(string email);
		Task<Tuple<string, User, ApiResponse>> TokenGenerateWithUDID(AccessDevice accessDevice);
		string DecodeJwtToken(string token);
		string DecodeAdminJwtToken(string token);
		string TokenGenerateSignUp(string email, Int64 userId);
		//bool IsValidToken(string token, Int64 userId);
	}
}
