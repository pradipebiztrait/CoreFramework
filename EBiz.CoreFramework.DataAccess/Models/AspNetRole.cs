using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class AspNetRole
    {
        [Key]
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
