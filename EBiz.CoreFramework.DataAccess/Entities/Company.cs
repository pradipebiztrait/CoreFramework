using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.DataAccess.Entities
{
    public class Company
    {
        [Key]
        public Int64 company_id { get; set; }
        public string company_name { get; set; }
        public string email_address { get; set; }
        public string contact_number { get; set; }
        public string whatsapp_number { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string website_url { get; set; }
        public string facebook_url { get; set; }
        public string google_url { get; set; }
        public string twitter_url { get; set; }
        public string instagram_url { get; set; }
        public string youtube_url { get; set; }
        public string company_logo { get; set; }
        public Int64? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public Int64? updated_by { get; set; }
        public DateTime? updated_on { get; set; }
    }
}
