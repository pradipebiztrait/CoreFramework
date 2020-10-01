using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class PushNotification
    {
        public PushNotificationContent notification { get; set; }
        public object data { get; set; }
        public string[] registration_ids { get; set; }
    }

    public class PushNotificationContent
    {
        public string title { get; set; }
        public string body { get; set; }
    }
}
