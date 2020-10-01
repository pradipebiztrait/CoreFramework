using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class SiteSettings
    {
        public string TokenString { get; set; }
        public string BaseUrl { get; set; }
        public int RequestTimeOut { get; set; }
        public string GoogleAuthClientId { get; set; }
        public string GoogleAuthClientSecret { get; set; }
        public string GoogleCalendarId { get; set; }
        public string GoogleCalendarTimeZone { get; set; }
    }

    public class SiteEmail
    {
        [Key]
        public Int64 site_email_id { get; set; }
        public string server_name { get; set; }
        public string port_number { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
        public string password { get; set; }
        public int? is_receiver { get; set; }
        public int? is_active { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        [NotMapped]
        public Int64? UserId { get; set; }
        [NotMapped]
        public bool is_status { get; set; }
    }

    public class SiteNotification
    {
        [Key]
        public Int64 notification_id { get; set; }
        public string iphone_key { get; set; }
        public string android_key { get; set; }
        public string send_url { get; set; }
        public int? is_active { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        [NotMapped]
        public Int64? UserId { get; set; }
    }

    public class SiteAWSProperty
    {
        [Key]
        public Int64 aws_property_id { get; set; }
        public string bucket_name { get; set; }
        public string access_key { get; set; }
        public string secret_key { get; set; }
        public string base_url { get; set; }
        public int? is_active { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
        [NotMapped]
        public Int64? UserId { get; set; }
    }

    public class SiteSettingModel
    {
        public SiteEmail site_email { get; set; }
        public SiteNotification site_notification { get; set; }
        public SiteAWSProperty site_aws_property { get; set; }
        public int? is_emailsetting_active { get; set; }
        public int? is_notification_active { get; set; }
    }
}
