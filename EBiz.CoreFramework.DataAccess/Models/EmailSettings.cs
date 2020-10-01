namespace EBiz.CoreFramework.DataAccess.Models
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Password { get; set; }
    }
}
