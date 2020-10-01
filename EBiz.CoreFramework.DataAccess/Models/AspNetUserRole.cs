using System;
using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class AspNetUserRole
    {
        [Key]
        public Int64 UserId { get; set; }
        public string RoleId { get; set; }
    }
}
