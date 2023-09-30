using API.Model.DTOs;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<AppointmentDTO> GetAppointmentById(string appointmentId);
        public List<Appointment> GetMentorAppointments(string mentorId);
        public Task<Appointment> CreateAppointment(AppointmentDTO model, string accountId);
        public Task<Appointment> UpdateAppointment(AppointmentDTO model, string appointmentId);
        public Task<List<AppointmentDTO>> GetUserAppointment(string accountId);
        public Task<bool> DeleteAppointment(string appointmentId);
        public Task<bool> VerifyAppointment(string code);
    }
}
