using System.Threading.Tasks;

namespace Mentore.Services
{
    public interface IEmailSender
    {
        public Task SendEmailVerificationAsync(string toEmail, string code, string emailFor);
    }
}