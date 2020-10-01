using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
    }
}
