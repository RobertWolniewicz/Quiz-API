using Quiz_API.Entity;
using System.Net;
using System.Net.Mail;

namespace Quiz_API
{
    public class EmailSender
    {
        public static void send(EmailParams emailparams, List<string> addreses, int value)
        {
            var smtpClient = new SmtpClient(emailparams.smtpSerwer, emailparams.smtpPort);
            smtpClient.Credentials = new NetworkCredential(emailparams.email, emailparams.password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailparams.email);
            foreach(string addres in addreses)
            {
                mailMessage.To.Add(addres);
            }
            
            mailMessage.Subject = emailparams.subject;
            mailMessage.Body = emailparams.body.Replace("#", value.ToString());

            smtpClient.Send(mailMessage);
        }
    }
}
