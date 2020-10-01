using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
	[ScopedService]
    public class SmsService : ISmsService
    {
        private readonly SmsAuthantication _smsAuthantication;
        public SmsService(IOptions<SmsAuthantication> smsAuthantication)
        {
            _smsAuthantication = smsAuthantication.Value;
        }
        public async Task SendTextMessage(int otp)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            TwilioClient.Init(_smsAuthantication.AccountSid, _smsAuthantication.AuthToken);

            await MessageResource.CreateAsync(
                body: "Your verification code is "+ otp + ".",
                from: new Twilio.Types.PhoneNumber(_smsAuthantication.Sender),
                to: new Twilio.Types.PhoneNumber(_smsAuthantication.Receiver)
            );
        }

        public async Task SendLoginOtp(int otp,string userName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            TwilioClient.Init(_smsAuthantication.AccountSid, _smsAuthantication.AuthToken);

            await MessageResource.CreateAsync(
                body: "Your verification code is " + otp + ".",
                from: new Twilio.Types.PhoneNumber(_smsAuthantication.Sender),
                to: new Twilio.Types.PhoneNumber(_smsAuthantication.Receiver)
            );
        }
    }
}
