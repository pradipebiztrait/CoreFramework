using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EBiz.CoreFramework.DataAccess.Models
{
	public class GoogleUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string LoginProvider { get; set; }
		public string ProviderKey { get; set; }
		public string ImagePath { get; set; }
		public bool IsGoogleUser { get; set; }
		public string AccessToken { get; set; }
		public string DeviceToken { get; set; }
		public int DeviceType { get; set; }
		public string Udid { get; set; }
		public int UserType { get; set; }
	}

	public class UserProfile
	{
		public Int64 UserId { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MobileNumber { get; set; }
		public string ImagePath { get; set; }
		[NotMapped]
		public string ImageName { get; set; }
		public IFormFile ImageFile { get; set; }
	}

	public class User
	{        
        [Key]
		public Int64 UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string MobileNumber { get; set; }
		public string EmailAddress { get; set; }
		public string Address { get; set; }
		public Int64? CountryId { get; set; }
		public Int64? StateId { get; set; }
		public Int64? CityId { get; set; }
		public string PostalCode { get; set; }
		public int? IsActive { get; set; }       
        public int? IsDelete { get; set; }
		public string Token { get; set; }
		public int? OTP { get; set; }
		public string Password { get; set; }
		public string ImagePath { get; set; }
		public int? Gender { get; set; }
		public Int64? RoleId { get; set; }
		public string DeviceToken { get; set; }
		public int? DeviceType { get; set; }  
		public int? IsAdmin { get; set; }  
        public string UdId { get; set; }
		public Int64? CreatedBy { get; set; }
		public Int64? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [NotMapped]
		public IFormFile ProfileImage { get; set; }
		[NotMapped]
		public string ImageName { get; set; }
		[NotMapped]
		public string CityName { get; set; }
		[NotMapped]
		public string CountryName { get; set; }
		[NotMapped]
		public string StateName { get; set; }
		[NotMapped]
		public int IsPaid { get; set; }
        [NotMapped]
        public List<Roles> Roles { get; set; }
    }

	public class SubAdminUser
	{
		public Int64 UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string MobileNumber { get; set; }
		public string ImagePath { get; set; }
		public Int64 AdminId { get; set; }
	}

	public class ContactUs
	{
		public Int64 UserId { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Message { get; set; }
		public string PhoneNumber { get; set; }
	}

    public class LoackscreenUser
    {
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
    }

    public class UserFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? IsActive { get; set; }
    }
}
