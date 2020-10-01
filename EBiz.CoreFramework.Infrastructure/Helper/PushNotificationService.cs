using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using EBiz.CoreFramework.DataAccess.Models;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.DbContext;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
    [ScopedService]
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PushNotificationService(   
            IConfiguration configuration
            , ApplicationDbContext context)
        {       
            _configuration = configuration;
            _context = context;
        }

        public async Task<bool> SendAsync(PushNotification model)
        {
            bool sent = false;

            try
            {

                var _notification = _context.site_notification.FirstOrDefault();
                if(_notification != null && _notification.is_active == 1)
                {
                    if (model.registration_ids.Count() > 0)
                    {
                        var _req = new HttpRequestMessage(HttpMethod.Post, _notification.send_url);
                        _req.Headers.TryAddWithoutValidation("Authorization", "key=" + _notification.android_key);
                        _req.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                        using (var client = new HttpClient())
                        {
                            HttpResponseMessage result = await client.SendAsync(_req);
                            sent = result.IsSuccessStatusCode;
                        }
                    }
                }               

                return sent;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
