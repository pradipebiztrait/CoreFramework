using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
    public interface IGoogleService : IScopedService
	{
        GoogleUserResponseModel GetUserProfile(string accesstoken);
        Task<string> GetAccessToken();
    }
}
