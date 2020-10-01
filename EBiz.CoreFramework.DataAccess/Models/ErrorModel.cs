using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
    }
}
