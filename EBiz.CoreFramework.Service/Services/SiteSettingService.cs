using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Repository.Repositories;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
    [ScopedService]
    public class SiteSettingService : ISiteSettingService
    {
        private readonly ISiteSettingRepository _repository;

        public SiteSettingService(ISiteSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<SiteSettingModel> GetSiteSettingsAsync()
        {
            var model = new SiteSettingModel();

            var _emailSettings = await _repository.GetEmailSettingsAsync();
            if (_emailSettings != null)
            {
                model.site_email = (SiteEmail)_emailSettings.Data;
                model.is_emailsetting_active = model.site_email.is_active;
            }

            var _notificationSettings = await _repository.GetPushNotificationSettingsAsync();
            if (_notificationSettings != null)
            {
                model.site_notification = (SiteNotification)_notificationSettings.Data;
                model.is_notification_active = model.site_notification.is_active;
            }

            var _awsSettings = await _repository.GetAWSPropertySettingsAsync();
            if (_awsSettings != null) model.site_aws_property = (SiteAWSProperty)_awsSettings.Data;

            return model;
        }

        public async Task<ApiResponse> GetEmailSettingsAsync() => await _repository.GetEmailSettingsAsync();
		public async Task<ApiResponse> SaveEmailSettingsAsync(SiteEmail model) => await _repository.SaveEmailSettingsAsync(model);

        public async Task<ApiResponse> GetPushNotificationSettingsAsync() => await _repository.GetPushNotificationSettingsAsync();
        public async Task<ApiResponse> SavePushNotificationSettingsAsync(SiteNotification model) => await _repository.SavePushNotificationSettingsAsync(model);

        public async Task<ApiResponse> GetAWSPropertySettingsAsync() => await _repository.GetAWSPropertySettingsAsync();
        public async Task<ApiResponse> SaveAWSPropertySettingsAsync(SiteAWSProperty model) => await _repository.SaveAWSPropertySettingsAsync(model);

    }
}
