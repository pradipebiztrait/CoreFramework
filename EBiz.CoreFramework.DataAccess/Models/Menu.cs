using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class Menu
    {
        [Key]
        public Int64 menu_id { get; set; }
        public string menu_title { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
        public Int64? parent_menu_id { get; set; }
        public int? is_active { get; set; }
        public int? sort_order { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        [NotMapped]
        public List<Menu> SubMenu { get; set; }
        [NotMapped]
        public Int64 UserId { get; set; }
        [NotMapped]
        public bool is_status { get; set; }
    }

    public class MenuFilter
    {
        public string menu_title { get; set; }
        public string menu_url { get; set; }
    }
}
