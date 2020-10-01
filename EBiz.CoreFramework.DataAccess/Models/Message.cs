using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class Message
    {
        public string[] registration_ids { get; set; }
        public AccessNotification notification { get; set; }
        public object data { get; set; }
    }
}
