using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Entities
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
