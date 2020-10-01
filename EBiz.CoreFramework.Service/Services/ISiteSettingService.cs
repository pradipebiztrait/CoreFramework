using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	public interface ISiteSettingService : IScopedService
	{
        Task<SiteSettingModel> GetSiteSettingsAsync();

        Task<ApiResponse> GetEmailSettingsAsync();
        Task<ApiResponse> SaveEmailSettingsAsync(SiteEmail model);

        Task<ApiResponse> GetPushNotificationSettingsAsync();
        Task<ApiResponse> SavePushNotificationSettingsAsync(SiteNotification model);

        Task<ApiResponse> GetAWSPropertySettingsAsync();
        Task<ApiResponse> SaveAWSPropertySettingsAsync(SiteAWSProperty model);
    }
}
