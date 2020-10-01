using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class RolePermission
    {
        [Key]
        public Int64 role_permission_id { get; set; }
        public Int64? role_id { get; set; }
        public Int64? menu_id { get; set; }
        public int is_view { get; set; }
        public int is_add { get; set; }
        public int is_edit { get; set; }
        public int is_delete { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
    }
}
