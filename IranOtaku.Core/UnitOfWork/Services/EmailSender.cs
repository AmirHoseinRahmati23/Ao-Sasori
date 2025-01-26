using IranOtaku.Core.UnitOfWork.Repositories;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IranOtaku.Core.UnitOfWork.Services
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            using var client = new SmtpClient();
            var credentials = new NetworkCredential()
            {
                UserName = "AoSasoriGroup", // without @gmail.com
                Password = "AoSasori@Arman@Amir@Team"
            };

            client.Credentials = credentials;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            using var emailMessage = new MailMessage()
            {
                To = { new MailAddress(toEmail) },
                From = new MailAddress("AoSasoriGroup@gamil.com"), // with @gmail.com
                Subject = subject,
                Body = message,
                IsBodyHtml = isMessageHtml
            };

            client.Send(emailMessage);

            return Task.CompletedTask;
        }
    }
}
