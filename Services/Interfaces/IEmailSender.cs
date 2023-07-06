using API.Model.DTOs;
using System.Threading.Tasks;

namespace Mentore.Services
{
    public interface IEmailSender
    {
        public Task SendEmailVerificationAsync(string toEmail, string code, string emailFor);
        public Task SendEmailPaySuccessAsync(string toEmail, string code, string workshopName);
        public Task SendEmailAppointment(AppointmentEmailDTO model, string action);
    }
}