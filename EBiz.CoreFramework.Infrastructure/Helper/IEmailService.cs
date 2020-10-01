using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
    public interface IEmailService : IScopedService
	{
        Task SendEmailAsync(string email, string subject, string message, string attach);
        void SendMailbyThread(string to, string subject, string body, string attach);
		Task<ApiResponse> SendAsync(string to, string subject, string body, string attach);
		string CreateEmailBody(string resetLink, string template);
	}

}
