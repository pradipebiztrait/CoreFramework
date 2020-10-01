using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name= "EmailSettingComponent")]
    public class EmailSettingComponent : ViewComponent
    {
        private readonly ISiteSettingService _service;
        public EmailSettingComponent(ISiteSettingService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SiteEmail();
            var result = await _service.GetEmailSettingsAsync();
            if (result.Data != null)
            {
                model.site_email_id = result.Data.site_email_id;
                model.server_name = result.Data.server_name;
                model.port_number = result.Data.port_number;
                model.sender = result.Data.sender;
                model.receiver = result.Data.receiver;
                model.password = result.Data.password;
                model.is_receiver = result.Data.is_receiver;
                model.is_status = (result.Data.is_receiver == 1 ? true : false);
            }
            
            return await Task.FromResult((IViewComponentResult)View("Default",model));
        }
    }

    [ViewComponent(Name = "PushNotificationSettingComponent")]
    public class PushNotificationSettingComponent : ViewComponent
    {
        private readonly ISiteSettingService _service;
        public PushNotificationSettingComponent(ISiteSettingService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SiteNotification();
            var result = await _service.GetPushNotificationSettingsAsync();
            if (result.Data != null)
            {
                model.notification_id = result.Data.notification_id;
                model.iphone_key = result.Data.iphone_key;
                model.android_key = result.Data.android_key;
                model.send_url = result.Data.send_url;
                //model.is_active = result.Data.is_active;
                //model.is_status = (result.Data.is_active == 1 ? true : false);
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }

    [ViewComponent(Name = "AWSPropertySettingComponent")]
    public class AWSPropertySettingComponent : ViewComponent
    {
        private readonly ISiteSettingService _service;
        public AWSPropertySettingComponent(ISiteSettingService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SiteAWSProperty();
            var result = await _service.GetAWSPropertySettingsAsync();
            if (result.Data != null)
            {
                model.aws_property_id = result.Data.aws_property_id;
                model.bucket_name = result.Data.bucket_name;
                model.access_key = result.Data.access_key;
                model.secret_key = result.Data.secret_key;
                model.base_url = result.Data.base_url;
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
