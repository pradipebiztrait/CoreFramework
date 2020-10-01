using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using Dapper;
using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	[ScopedService]
	public class StaffRepository : IStaffRepository
	{
		private readonly IDapperService _dapperService;
		private readonly ApplicationDbContext _context;

		public StaffRepository(
			IDapperService dapperService,
			ApplicationDbContext context)
		{
			_dapperService = dapperService;
			_context = context;
		}

        public async Task<ApiResponse> ListByFiltersAsync(FilterRequest request)
        {
            var _apiResponse = new ApiResponse();
            var _param = new DynamicParameters();

            _param.Add("@oSkip", request.Skip, DbType.Int32);
            _param.Add("@oTake", request.Take, DbType.Int32);
            _param.Add("@oSearchText", request.SearchText, DbType.String);
            _param.Add("@oSortCol", request.SortCol, DbType.String);
            _param.Add("@oSortDir", request.SortDir, DbType.String);
            _param.Add("@oMessage", null, DbType.String, ParameterDirection.Output);
            _param.Add("@oStatus", 0, DbType.Int16, ParameterDirection.Output);

            using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetAllStaffByFilters", _param, CommandType.StoredProcedure)).Result)
            {
                _apiResponse.Data = gridReader.Read<User>().ToList();
                _apiResponse.TotalRecord = gridReader.Read<Int32>().FirstOrDefault();
            }

            _apiResponse.Message = _param.Get<string>("oMessage");
            _apiResponse.Status = _param.Get<Int16>("oStatus") == 1 ? true : false;

            return _apiResponse;
        }

		public async Task<User> GetById(Int64 userId)
		{
			var _param = new DynamicParameters();
			_param.Add("@oUserId", userId, DbType.Int64);
			return await Task.FromResult(_dapperService.Get<User>("API_GetUserById", _param, CommandType.StoredProcedure));
		}

		public async Task<User> GetUserDetailById(Int64 userId)
		{
			var user = new User();
			var _param = new DynamicParameters();
			_param.Add("@oUserId", userId, DbType.Int64);

			using (var gridReader = Task.FromResult(await _dapperService.GetMultipleAsync("API_GetUserDetailById", _param, CommandType.StoredProcedure)).Result)
			{
				user = gridReader.Read<User>().FirstOrDefault();
			}

			return user;

		}

		public async Task<ApiResponse> IsActiveUser(Int64 userId, int isActive)
		{
			var _apiResponse = new ApiResponse();

			var _param = new DynamicParameters();
			_param.Add("@oUserId", userId, DbType.Int64);
			_param.Add("@oIsActive", isActive, DbType.Int16);
			_param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			_param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);
			await _dapperService.GetAsync<User>("API_IsActiveUser", _param, CommandType.StoredProcedure);

			_apiResponse.Message = _param.Get<string>("SuccessMsg");
			_apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiResponse;
		}

		public async Task<ApiResponse> SaveUserProfileAsync(UserProfile model)
		{
			var _apiResponse = new ApiResponse();

			var _param = new DynamicParameters();
			_param.Add("@oUserId", model.UserId, DbType.Int64);
			_param.Add("@oEmail", model.Email, DbType.String);
			_param.Add("@oFirstName", model.FirstName, DbType.String);
			_param.Add("@oLastName", model.LastName, DbType.String);
			_param.Add("@oImagePath", model.ImagePath, DbType.String);
			_param.Add("@oMobileNumber", model.MobileNumber, DbType.String);
			_param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
			_param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

			await _dapperService.GetAsync<UserProfile>("API_SaveAdminProfile", _param, CommandType.StoredProcedure);

			_apiResponse.Message = _param.Get<string>("SuccessMsg");
			_apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

			return _apiResponse;
		}

        public async Task<ApiResponse> SaveStaffAsync(User model)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@uUserId", model.UserId, DbType.Int64);
            _param.Add("@uEmailAddress", model.EmailAddress, DbType.String);
            _param.Add("@uPassword", model.Password, DbType.String);
            _param.Add("@uFirstName", model.FirstName, DbType.String);
            _param.Add("@uLastName", model.LastName, DbType.String);
            _param.Add("@uMobileNumber", model.MobileNumber, DbType.String);
            _param.Add("@uToken", model.Token, DbType.String);
            _param.Add("@uRoleId", model.RoleId, DbType.Int32);
            _param.Add("@uMessage", null, DbType.String, ParameterDirection.Output);
            _param.Add("@uStatus", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = Task.FromResult(await _dapperService.GetAsync<User>("API_SaveStaff", _param, CommandType.StoredProcedure)).Result;

            _apiResponse.Message = _param.Get<string>("uMessage");
            _apiResponse.Status = _param.Get<Int16>("uStatus") == 1 ? true : false;

            return _apiResponse;
        }

        public void UpdateUserImagePath(Int64 userId, string imagePath)
		{
			var _param = new DynamicParameters();
			_param.Add("@ouser_id", userId, DbType.Int64);
			_param.Add("@oprofile_photo", imagePath, DbType.String);
			_dapperService.Execute("API_UpdateProfilePicture", _param, CommandType.StoredProcedure);
		}

        public async Task<ApiResponse> DeleteAsync(Int64 id)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@oid", id, DbType.Int64);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = await _dapperService.GetAsync<User>("API_DeleteUser", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

        public async Task<ApiResponse> DeleteMultipleAsync(string ids)
        {
            var _apiResponse = new ApiResponse();

            var _param = new DynamicParameters();
            _param.Add("@oids", ids, DbType.String);
            _param.Add("@SuccessMsg", null, DbType.String, ParameterDirection.Output);
            _param.Add("@IsSuccess", 0, DbType.Int16, ParameterDirection.Output);

            _apiResponse.Data = await _dapperService.GetAsync<User>("API_DeleteMultipleUser", _param, CommandType.StoredProcedure);

            _apiResponse.Message = _param.Get<string>("SuccessMsg");
            _apiResponse.Status = _param.Get<Int16>("IsSuccess") == 1 ? true : false;

            return _apiResponse;
        }

		public async Task<ApiResponse> GetUserProfile(string token, Int64 id)
		{
			var _apiResponse = new ApiResponse();
			var _param = new DynamicParameters();
			_param.Add("@uUserId", id, DbType.Int64);
			_param.Add("@uToken", token, DbType.String);
			_param.Add("@uMessage", null, DbType.String, ParameterDirection.Output);
			_param.Add("@uStatus", 0, DbType.Int16, ParameterDirection.Output);

			_apiResponse.Data = await Task.FromResult(_dapperService.Get<User>("API_GetUserProfile", _param, CommandType.StoredProcedure));

			_apiResponse.Message = _param.Get<string>("uMessage");
			_apiResponse.Status = _param.Get<Int16>("uStatus") == 1 ? true : false;

			return _apiResponse;
		}

		public async Task<ApiResponse> UpdateUserProfileAsync(User model)
		{
			var _apiResponse = new ApiResponse();
			var _param = new DynamicParameters();
			_param.Add("@uUserId", model.UserId, DbType.Int64);
			_param.Add("@uToken", model.Token, DbType.String);
			_param.Add("@uFirstName", model.FirstName, DbType.String);
			_param.Add("@uLastName", model.LastName, DbType.String);
			_param.Add("@uCountryId", model.CountryId, DbType.Int64);
			_param.Add("@uStateId", model.StateId, DbType.Int64);
			_param.Add("@uCityId", model.CityId, DbType.Int64);
			_param.Add("@uDeviceToken", model.DeviceToken, DbType.String);
			_param.Add("@uDeviceType", model.DeviceType, DbType.Int32);
			_param.Add("@uUdId", model.UdId, DbType.String);
			_param.Add("@uMessage", null, DbType.String, ParameterDirection.Output);

			_param.Add("@uStatus", 0, DbType.Int16, ParameterDirection.Output);
			_apiResponse.Data = await Task.FromResult(_dapperService.Get<User>("API_UpdateUserProfile", _param, CommandType.StoredProcedure));

			_apiResponse.Message = _param.Get<string>("uMessage");
			_apiResponse.Status = _param.Get<Int16>("uStatus") == 1 ? true : false;

			return _apiResponse;
		}

	}
}