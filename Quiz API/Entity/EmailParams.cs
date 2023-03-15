namespace Quiz_API.Entity
{
    public class EmailParams
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string SmtpSerwer { get; set; }
        public int SmtpPort { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
