using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EBiz.CoreFramework.DataAccess.Entities
{

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
        public int DeviceType { get; set; }
        public int? IsAdmin { get; set; }
        public string UdId { get; set; }
        public Int64? CreatedBy { get; set; }
        public Int64? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
