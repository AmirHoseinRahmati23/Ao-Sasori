using System.Threading.Tasks;

namespace IranOtaku.Core.UnitOfWork.Repositories
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
