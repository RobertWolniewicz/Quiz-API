namespace Quiz_API.Entity
{
    public class EmailParams
    {
        public int id { get; set; }
        public string email { get; set; }
        public string smtpSerwer { get; set; }
        public int smtpPort { get; set; }
        public string password { get; set; }
        public string subject { get; set; }
        public string body { get; set; }

    }
}
