using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
    public interface ISmsService : IScopedService
	{
        Task SendTextMessage(int otp);
        Task SendLoginOtp(int otp, string userName);
    }
}
