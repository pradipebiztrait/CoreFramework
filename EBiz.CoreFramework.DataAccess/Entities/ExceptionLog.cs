using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class ExceptionLog
    {
        [Key]
        public Int64 exception_log_id { get; set; }
        public string message { get; set; }
        public string stack_trace { get; set; }
        public string ip_address { get; set; }
        public int? status_code { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
    }
}
