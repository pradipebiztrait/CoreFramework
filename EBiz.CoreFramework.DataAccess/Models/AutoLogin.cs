using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class AutoLogin
    {
        public string DeviceToken { get; set; }
        public Int64 DeviceType { get; set; }
        public string Udid { get; set; }

    }
}
