using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using EBiz.CoreFramework.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
	public interface IStaffRepository : IScopedService
	{
        Task<ApiResponse> ListByFiltersAsync(FilterRequest request);
        Task<User> GetById(Int64 userId);
        Task<User> GetUserDetailById(Int64 userId);
        Task<ApiResponse> IsActiveUser(Int64 userId, int isActive);
        Task<ApiResponse> SaveUserProfileAsync(UserProfile model);
        void UpdateUserImagePath(Int64 userId, string imagePath);
        Task<ApiResponse> DeleteAsync(Int64 id);
        Task<ApiResponse> DeleteMultipleAsync(string ids);
        Task<ApiResponse> GetUserProfile(string token, Int64 id);
        Task<ApiResponse> UpdateUserProfileAsync(User model);
        Task<ApiResponse> SaveStaffAsync(User model);
    }
}
