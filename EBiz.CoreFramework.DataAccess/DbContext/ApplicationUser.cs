using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBiz.CoreFramework.DataAccess.DbContext
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string UserId { get; set; }
    }
}
