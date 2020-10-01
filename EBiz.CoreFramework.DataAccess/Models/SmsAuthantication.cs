namespace EBiz.CoreFramework.DataAccess.Models
{
    public class SmsAuthantication
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
    }
}
