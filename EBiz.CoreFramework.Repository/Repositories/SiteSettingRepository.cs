using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        private readonly IDapperService _dapperService;
        private readonly ApplicationDbContext _context;

        public SiteSettingRepository(IDapperService dapperService, ApplicationDbContext context)
        {
            _dapperService = dapperService;
            _context = context;

        }

        //Email Settings
        public async Task<ApiResponse> GetEmailSettingsAsync()
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.site_email.FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> SaveEmailSettingsAsync(SiteEmail model)
        {
            var _apiRes = new ApiResponse();

            var result = await _context.site_email.FirstOrDefaultAsync();

            if (result != null)
            {
                result.server_name = model.server_name;
                result.port_number = model.port_number;
                result.sender = model.sender;
                result.receiver = model.receiver;
                result.password = model.password;
                result.is_receiver = model.is_receiver;
                result.updated_by = model.UserId;
                result.updated_on = DateTime.Now;

                _context.site_email.Update(result);
                _context.SaveChanges();
            }
            else
            {
                model.created_by = model.UserId;
                model.created_on = DateTime.Now;
                model.updated_by = model.UserId;
                model.updated_on = DateTime.Now;

                await _context.site_email.AddAsync(model);
                await _context.SaveChangesAsync();
            }

            _apiRes.Message = "Email site settings has been saved.";
            _apiRes.Status = true;

            return _apiRes;
        }

        //Push Notification Settings
        public async Task<ApiResponse> GetPushNotificationSettingsAsync()
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.site_notification.FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> SavePushNotificationSettingsAsync(SiteNotification model)
        {
            var _apiRes = new ApiResponse();

            var result = await _context.site_notification.FirstOrDefaultAsync();

            if (result != null)
            {
                result.iphone_key = model.iphone_key;
                result.android_key = model.android_key;
                result.send_url = model.send_url;
                result.is_active = model.is_active;
                result.updated_by = model.UserId;
                result.updated_on = DateTime.Now;

                _context.site_notification.Update(result);
                _context.SaveChanges();
            }
            else
            {
                model.created_by = model.UserId;
                model.created_on = DateTime.Now;
                model.updated_by = model.UserId;
                model.updated_on = DateTime.Now;

                await _context.site_notification.AddAsync(model);
                await _context.SaveChangesAsync();
            }

            _apiRes.Message = "Push Notification settings has been saved.";
            _apiRes.Status = true;

            return _apiRes;
        }

        //AWS Property Settings
        public async Task<ApiResponse> GetAWSPropertySettingsAsync()
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.site_aws_property.FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> SaveAWSPropertySettingsAsync(SiteAWSProperty model)
        {
            var _apiRes = new ApiResponse();

            var result = await _context.site_aws_property.FirstOrDefaultAsync();

            if (result != null)
            {
                result.bucket_name = model.bucket_name;
                result.access_key = model.access_key;
                result.secret_key = model.secret_key;
                result.base_url = model.base_url;
                result.is_active = model.is_active;
                result.updated_by = model.UserId;
                result.updated_on = DateTime.Now;

                _context.site_aws_property.Update(result);
                _context.SaveChanges();
            }
            else
            {
                model.created_by = model.UserId;
                model.created_on = DateTime.Now;
                model.updated_by = model.UserId;
                model.updated_on = DateTime.Now;

                await _context.site_aws_property.AddAsync(model);
                await _context.SaveChangesAsync();
            }

            _apiRes.Message = "AWS Property settings has been saved.";
            _apiRes.Status = true;

            return _apiRes;
        }
    }
}
