using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using EBiz.CoreFramework.Repository.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Service.Services
{
	[ScopedService]
	public class StaffService : IStaffService
    {
		private readonly IStaffRepository _repository;
		public readonly AWSProperty _awsProperty;
        private readonly SiteSettings _siteSettings;
        private readonly IImageHelper _imageHelper;
        private readonly SiteFolders _siteFolders;

        public StaffService(
            IStaffRepository repository
             , IOptions<SiteSettings> siteSettings
             , IOptions<SiteFolders> siteFolders
            , IImageHelper imageHelper
            , IOptions<AWSProperty> awsProperty)
		{
			_repository = repository;
            _siteSettings = siteSettings.Value;
            _awsProperty = awsProperty.Value;
            _siteFolders = siteFolders.Value;
            _imageHelper = imageHelper;
        }

        public async Task<ApiResponse> ListByFiltersAsync(FilterRequest request)
        {
            var result = await _repository.ListByFiltersAsync(request);
            if (result.Data != null)
            {
                foreach (var t in result.Data)
                {
                    if (!string.IsNullOrEmpty(t.ImagePath))
                    {
                        t.ImagePath = t.ImagePath != null ? _siteSettings.BaseUrl + _imageHelper.GetImagePath(t.ImagePath, _siteFolders.UserFolder).Replace("\\", "/") : "/img/default-user-black.svg";
                    }
                }
            }
            return result;
        }

		public async Task<User> GetById(Int64 userId)
		{
			return await _repository.GetById(userId);
		}

		public async Task<User> GetUserDetailById(Int64 userId)
		{
			return await _repository.GetUserDetailById(userId);

		}

		public async Task<ApiResponse> IsActiveUser(Int64 userId, int isActive)
		{
			var response = new ApiResponse();
			var data = await _repository.IsActiveUser(userId, isActive);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> SaveUserProfileAsync(UserProfile model)
		{
			var response = new ApiResponse();
			var data = await _repository.SaveUserProfileAsync(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> SaveStaffAsync(User model) => await _repository.SaveStaffAsync(model);

		public void UpdateUserImagePath(Int64 userId, string imagePath)
		{
			_repository.UpdateUserImagePath(userId, imagePath);
		}

		#region 'APIs'

        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
           return await _repository.DeleteMultipleAsync(ids);
        }

		public async Task<ApiResponse> GetProfile(string token,long id)
		{
			var response = new ApiResponse();

			var data = await _repository.GetUserProfile(token, id);

			if (data.Data != null)
			{
				if (!string.IsNullOrEmpty(data.Data.ImagePath))
				{
					data.Data.ImagePath = _awsProperty.BaseUrl + "/images/User/" + data.Data.UserId + "/" + data.Data.ImagePath;
				}
			}
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> UpdateUserProfileAsync(User model)
		{
			var response = new ApiResponse();
			var data = await _repository.UpdateUserProfileAsync(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		#endregion 'APIs'
	}
}
