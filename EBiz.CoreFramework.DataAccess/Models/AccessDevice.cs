using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class AccessDevice
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string deviceToken { get; set; }
        public int deviceType { get; set; }
        public string udid { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
        [NotMapped]
        public int RoleId { get; set; }
    }
}
