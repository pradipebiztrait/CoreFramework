using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
//using RestSharp;
//using Newtonsoft.Json;
//using Microsoft.AspNetCore.Mvc;
//using MimeKit;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.DbContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	[ScopedService]
    public class EmailService : IEmailService
    {
		private readonly IHostingEnvironment _environment;
		private readonly ApplicationDbContext _context;

		public EmailService(IHostingEnvironment environment, ApplicationDbContext context)
        {
			_environment = environment;
            _context = context;

        }

		public Task SendEmailAsync(string to, string subject, string body, string attach)
		{
			try
			{
                var emailConfig = _context.site_email.FirstOrDefaultAsync();

                if(emailConfig.Result != null)

                {
                    if(emailConfig.Result.is_active == 1)
                    {

                        to = emailConfig.Result.is_receiver == 1 ? emailConfig.Result.receiver : to ;

                        return Task.Run(() =>
                        {
                            MailMessage _mail = new MailMessage();
                            _mail.From = new MailAddress(emailConfig.Result.sender, "Core Project");
                            _mail.To.Add(to);

                            if (!string.IsNullOrEmpty(attach))
                            {
                                Attachment attachment;
                                attachment = new Attachment(attach);
                                _mail.Attachments.Add(attachment);
                            }

                            _mail.CC.Add(to);
                            _mail.Subject = subject;
                            _mail.IsBodyHtml = true;
                            _mail.Body = body;

                            NetworkCredential _loginInfo = new NetworkCredential(emailConfig.Result.sender, emailConfig.Result.password);

                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = emailConfig.Result.server_name;
                            smtp.Port = Convert.ToInt32(emailConfig.Result.port_number);
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = _loginInfo;
                            smtp.SendMailAsync(_mail);
                        });
                    }

                }

                return null;
            }
			catch (Exception ex)
			{
				throw new InvalidOperationException(ex.Message);
			}
		}

		public async Task<ApiResponse> SendAsync(string to, string subject, string body, string attach)
		{
			var res = new ApiResponse();
			try
			{
                var emailConfig = _context.site_email.FirstOrDefaultAsync();

                MailMessage _mail = new MailMessage();
				_mail.From = new MailAddress(emailConfig.Result.sender, "Core Project");
				_mail.To.Add(to);

				if (!string.IsNullOrEmpty(attach))
				{
					Attachment attachment;
					attachment = new Attachment(attach);
					_mail.Attachments.Add(attachment);
				}

				_mail.CC.Add(to);
				_mail.Subject = subject;
				_mail.IsBodyHtml = true;
				_mail.Body = body;

				NetworkCredential _loginInfo = new NetworkCredential(emailConfig.Result.sender, emailConfig.Result.password);

				SmtpClient smtp = new SmtpClient();
				smtp.Host = emailConfig.Result.server_name;
				smtp.Port = Convert.ToInt32(emailConfig.Result.port_number);
                smtp.EnableSsl = true;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = _loginInfo;
				await smtp.SendMailAsync(_mail);

				res.Status = true;
				res.Message = "Success";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return res;
		}

		public void SendMailbyThread(string to, string subject, string body, string attach)
        {
            ThreadStart threadStart = delegate () { SendEmailAsync(to, subject, body, attach); };
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

		public string CreateEmailBody(string resetLink,string template)
		{

			string body = string.Empty;

			var templatePath = _environment.WebRootPath
							+ Path.DirectorySeparatorChar.ToString()
							+ "Templates"
							+ Path.DirectorySeparatorChar.ToString()
							+ template;

			using (StreamReader reader = new StreamReader(templatePath))
			{
				body = reader.ReadToEnd();
			}
			body = body.Replace("{resetLink}", resetLink);

			return body;
		}
	}
}
