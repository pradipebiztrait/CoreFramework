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
	public class UserService : IUserService
	{
		private readonly IUserRepository _repository;
		public readonly AWSProperty _awsProperty;
        private readonly SiteSettings _siteSettings;
        private readonly IImageHelper _imageHelper;
        private readonly SiteFolders _siteFolders;

        public UserService(
			IUserRepository repository
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

		public async Task<ApiResponse> GetAllAsync()
		{
			var response = new ApiResponse();
			var data = await _repository.GetAllAsync();
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

        public async Task<ApiResponse> GetAllUserByFiltersAsync(FilterRequest request)
        {
            var result = await _repository.GetAllUserByFiltersAsync(request);
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

        public async Task<ApiResponse> GetAllUserForNotification()
		{
			var response = new ApiResponse();
			var data = await _repository.GetAllUserForNotification();
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<User> GetById(Int64 userId)
		{
			return await _repository.GetById(userId);
		}

		public async Task<User> GetUserDetailById(Int64 userId)
		{
			return await _repository.GetUserDetailById(userId);

		}

		public async Task<User> GetSubAdminUserById(Int64 id)
		{
			return await _repository.GetSubAdminUserById(id);
		}

		public async Task<ApiResponse> GetUserByEmailAndPassword(string userName, string password)
		{
			var response = new ApiResponse();
			var data = await _repository.GetUserByEmailAndPassword(userName, password);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> GetUserByEmail(string email)
		{
			var response = new ApiResponse();
			var data = await _repository.GetUserByEmail(email);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
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

		public async Task<ApiResponse> SaveSubAdminAsync(SubAdminUser model)
		{
			var response = new ApiResponse();

			model.Password = (!string.IsNullOrEmpty(model.Password) ? new Security().Encrypt(model.Password.Trim()) : "");

			var data = await _repository.SaveSubAdminAsync(model);

			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> SaveUserAsync(User model)
		{
			var response = new ApiResponse();
			var data = await _repository.SaveUserAsync(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> ActiveUser(Int64 user_id)
		{
			var response = new ApiResponse();
			var data = await _repository.ActiveUser(user_id);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public void UpdateUserImagePath(Int64 userId, string imagePath)
		{
			_repository.UpdateUserImagePath(userId, imagePath);
		}

		#region 'APIs'

		public async Task<ApiResponse> AdminChangePasswordAsync(ChangePassword model)
		{
			var response = new ApiResponse();
			var data = await _repository.AdminChangePasswordAsync(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> GetAllSubAdminAsync()
		{
			var response = new ApiResponse();
			var data = await _repository.GetAllSubAdminAsync();
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> DeleteSubAdminByIdAsync(Int64 id)
		{
			var response = new ApiResponse();
			var data = await _repository.DeleteSubAdminByIdAsync(id);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
           return await _repository.DeleteMultipleAsync(ids);
        }

        public async Task<ApiResponse> GetAccessDevice(AccessDevice model)
		{

			var response = new ApiResponse();

			model.Password = new Security().Encrypt(model.Password.Trim());

			var data = await _repository.GetAccessDevice(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> SignUpAsync(User model)
		{
			var response = new ApiResponse();

			model.Password = new Security().Encrypt(model.Password);

			var data = await _repository.SignUpAsync(model);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
		}

		public async Task<ApiResponse> UpdateUserToken(AccessDevice request, Int64 userId, string token)
		{
			var response = new ApiResponse();
			var data = await _repository.UpdateUserToken(request, userId, token);
			response.Data = data.Data;
			response.Message = data.Message;
			response.Status = data.Status;

			return response;
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
