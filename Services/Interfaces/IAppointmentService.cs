using API.Model.DTOs;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IAppointmentService
    {
        public List<Appointment> GetMentorAppointments(string mentorId);
        public Task<Appointment> CreateAppointment(AppointmentDTO model, string accountId);
        public Task<Appointment> UpdateAppointment(AppointmentDTO model, string appointmentId);
        public List<Appointment> GetUserAppointment(string accountId);
        public Task<bool> DeleteAppointment(string appointmentId);
    }
}
