using Quiz_API.Entity;
using System.Net;
using System.Net.Mail;

namespace Quiz_API
{
    public class EmailSender
    {
        public static void send(EmailParams emailparams, List<string> addreses, int value)
        {
            var smtpClient = new SmtpClient(emailparams.SmtpSerwer, emailparams.SmtpPort);
            smtpClient.Credentials = new NetworkCredential(emailparams.Email, emailparams.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailparams.Email);
            foreach (string addres in addreses)
            {
                mailMessage.To.Add(addres);
            }

            mailMessage.Subject = emailparams.Subject;
            mailMessage.Body = emailparams.Body.Replace("#", value.ToString());

            smtpClient.Send(mailMessage);
        }
    }
}
