using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IUserRepository : IScopedService
	{
		Task<ApiResponse> GetAllUserForNotification();
		Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetAllUserByFiltersAsync(FilterRequest request);
        Task<ApiResponse> ListByFiltersAsync(FilterRequest request);
        Task<User> GetById(Int64 userId);
		Task<User> GetSubAdminUserById(Int64 id);
		Task<ApiResponse> GetUserByEmailAndPassword(string emailId, string password);
		Task<ApiResponse> IsActiveUser(Int64 userId, int isActive);
		Task<ApiResponse> SaveUserProfileAsync(UserProfile model);
		Task<ApiResponse> SaveSubAdminAsync(SubAdminUser model);
		Task<ApiResponse> SaveUserAsync(User model);
		void UpdateUserImagePath(Int64 userId, string imagePath);
		Task<ApiResponse> AdminChangePasswordAsync(ChangePassword model);
		Task<ApiResponse> GetUserByEmail(string email);
		Task<ApiResponse> ActiveUser(Int64 user_id);
		Task<ApiResponse> GetAllSubAdminAsync();
		Task<ApiResponse> DeleteSubAdminByIdAsync(Int64 id);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
        Task<User> GetUserDetailById(Int64 userId);
		Task<ApiResponse> GetAccessDevice(AccessDevice model);
		Task<ApiResponse> UpdateUserToken(AccessDevice request, Int64 userId, string token);
		Task<ApiResponse> SignUpAsync(User model);
		Task<ApiResponse> GetUserProfile(string token, Int64 id);
		Task<ApiResponse> UpdateUserProfileAsync(User model);
	}
}
