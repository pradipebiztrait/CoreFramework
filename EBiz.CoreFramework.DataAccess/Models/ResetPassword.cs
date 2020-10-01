using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class ResetPassword
    {
		[Required(ErrorMessage = "Invalid username.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Please enter password.")]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "Password must have {2} character", MinimumLength = 6)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Please enter confirm password")]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

		[NotMapped]
		public string Token { get; set; }
        [NotMapped]
        public string CurrentPassword { get; set; }
        [NotMapped]
        public Int64 UserId { get; set; }
    }
}
