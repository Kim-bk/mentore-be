using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Services.Interfaces;
using AutoMapper;
using Castle.Core.Internal;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Repositories;
using Mentore.Services;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AppointmentService : BaseService, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _map;
        private readonly IUserRepository _userRepo;
        private readonly IMenteeRepository _menteeRepo;
        private readonly IMentorRepository _mentorRepo;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapperCustom mapper,
            IAppointmentRepository appointmentRepo,
            IEmailSender emailSender,
            IMenteeRepository menteeRepo,
            IMentorRepository mentorRepo,
            IUserRepository userRepo,
            IMapper map) : base(unitOfWork, mapper)
        {
            _map = map;
            _appointmentRepo = appointmentRepo;
            _menteeRepo = menteeRepo;
            _mentorRepo = mentorRepo;
            _userRepo = userRepo;
            _emailSender = emailSender;
        }

        private bool CheckValidAppointment(AppointmentDTO model)
        {
            // Check any appointment has been created for mentor or mentee in that time
            var listMentorAppointments = _appointmentRepo.GetQuery
                (_ => _.DateStart.Date == Convert.ToDateTime(model.DateStart).Date
                && _.MentorId == model.MentorId
                && !_.IsDeleted && _.IsVerified ).ToList();
           
            if (!listMentorAppointments.IsNullOrEmpty())
            {
                foreach(var appt in listMentorAppointments)
                {
                    if (!IsOutsideDuration(appt, model.TimeStart, model.Duration))
                        return false;
                }    
            }    

            var listMenteeAppointments = _appointmentRepo.GetQuery
              (_ => _.DateStart.Date == Convert.ToDateTime(model.DateStart).Date 
              && _.MenteeId == model.MenteeId
              && !_.IsDeleted && _.IsVerified).ToList();

            if (!listMenteeAppointments.IsNullOrEmpty())
            {
                foreach (var appt in listMentorAppointments)
                {
                    if (!IsOutsideDuration(appt, model.TimeStart, model.Duration))
                        return false;
                }
            }    

            return true;
        }

        private static bool IsOutsideDuration(Appointment appt, string timeStart, int duration)
        {
            var inpHour = Convert.ToInt32(timeStart.Split(":")[0]) * 60;
            var inpMin = Convert.ToInt32(timeStart.Split(":")[1]);
            var apptHour = Convert.ToInt32(appt.TimeStart.Split(":")[0]) * 60;
            var apptMin = Convert.ToInt32(appt.TimeStart.Split(":")[1]);

            var inpTotal = inpHour + inpMin;
            var apptTotal = apptHour + apptMin;
            if (inpTotal <= apptTotal)
            {
                if (apptTotal - inpTotal < duration)
                    return false;
            }
            else
            {
                if (inpTotal - apptTotal < appt.Duration)
                   return false;
            }

            return true;
        }

        public async Task<Appointment> CreateAppointment(AppointmentDTO model, string accountId)
        {
            if (!CheckValidAppointment(model))
                return null;

            var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == accountId);

            var appointment = new Appointment
            {
                Title = model.Title,
                Detail = model.Detail,
                DateStart = Convert.ToDateTime(model.DateStart),
                MenteeId = mentee.Id,
                MentorId = model.MentorId,
                TimeStart = model.TimeStart,
                IsVerified = false,
                VerifiedCode = Guid.NewGuid().ToString(),
                LinkGoogleMeet = model.LinkGoogleMeet,
                Duration = model.Duration,
            };

            await _appointmentRepo.AddAsync(appointment);

            // Send email notification for mentor
            var mentor = await _mentorRepo.FindAsync(_ => _.Id == model.MentorId);
            var emailModel = new AppointmentEmailDTO
            { 
                Details = appointment.Detail,
                Title = appointment.Title,
                LinkGoogleMeet = appointment.LinkGoogleMeet,
                MenteeName = mentee.Name,
                MentorEmail = mentor.Email,
                DateTime = $"{model.TimeStart} ngày {model.DateStart:dd/MM/yyyy}",
                VerifiedCode = appointment.VerifiedCode
            };

            await _emailSender.SendEmailAppointment(emailModel, "createAppointment");
            await _unitOfWork.CommitTransaction();

            return appointment;
        }

        public async Task<bool> DeleteAppointment(string appointmentId)
        {
            var appointment = await _appointmentRepo.FindAsync(_ => _.Id == appointmentId);
            appointment.IsDeleted = true;
            _appointmentRepo.Update(appointment);

            // Send email cancel appointment
            var mentee = await _menteeRepo.FindAsync(_ => _.Id == appointment.MenteeId);
            var mentor = await _mentorRepo.FindAsync(_ => _.Id == appointment.MentorId);
            var emailModel = new AppointmentEmailDTO
            {
                MenteeName = mentee.Name,
                Title = appointment.Title,
                DateTime = $"{appointment.TimeStart} ngày {appointment.DateStart:dd/MM/yyyy}",
                Details = appointment.Detail,
                MenteeEmail = mentee.Email,
                MentorEmail = mentor.Email,
            };

            await _emailSender.SendEmailAppointment(emailModel, "cancelAppointment");
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public List<Appointment> GetMentorAppointments(string mentorId)
        {
            return _appointmentRepo.GetQuery(_ => _.MentorId == mentorId && !_.IsDeleted && _.IsVerified).ToList();
        }

        public async Task<List<AppointmentDTO>> GetUserAppointment(string accountId)
        {
            var rs = new List<AppointmentDTO>();
            var user = await _userRepo.FindAsync(_ => _.Id == accountId);
            if (user.UserGroupId == "MENTEE")
            {
                // Mentee appointmentss
                var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == accountId);
                var menteeAppointments = _appointmentRepo.GetQuery(_ => _.MenteeId == mentee.Id
                            && !_.IsDeleted && _.IsVerified
                            && _.DateStart >= DateTime.Now)
                            .OrderByDescending(_ => _.DateStart).ToList();

                foreach(var appt in menteeAppointments)
                {
                    var apptDto = _map.Map<AppointmentDTO>(appt);
                    apptDto.DateStart = appt.DateStart.ToString("yyyy-MM-dd");
                    rs.Add(apptDto);
                }    
            }
            if (user.UserGroupId == "MENTOR")
            {
                // Mentor appointments
                var mentor = await _mentorRepo.FindAsync(_ => _.AccountId == accountId);
                var mentorAppointments = _appointmentRepo.GetQuery(_ => _.MentorId == mentor.Id
                            && !_.IsDeleted && _.IsVerified
                            && _.DateStart >= DateTime.Now)
                            .OrderByDescending(_ => _.DateStart).ToList();

                foreach (var appt in mentorAppointments)
                {
                    var apptDto = _map.Map<AppointmentDTO>(appt);
                    apptDto.DateStart = appt.DateStart.ToString("yyyy-MM-dd");
                    rs.Add(apptDto);
                }
            }    

            return rs;
        }

        public async Task<Appointment> UpdateAppointment(AppointmentDTO model, string appointmentId)
        {
            if (!CheckValidAppointment(model))
                return null;

            var appointment = await _appointmentRepo.FindAsync(_ => _.Id == appointmentId);
            var mentee = await _menteeRepo.FindAsync(_ => _.Id == appointment.MenteeId);
            var mentor = await _mentorRepo.FindAsync(_ => _.Id == appointment.MentorId);

            appointment.Title = model.Title ?? appointment.Title;
            appointment.Detail = model.Detail ?? appointment.Detail;
            appointment.DateStart = model.DateStart != null ? Convert.ToDateTime(model.DateStart) : appointment.DateStart;
            appointment.TimeStart = model.TimeStart ?? appointment.TimeStart;
            appointment.LinkGoogleMeet = model.LinkGoogleMeet ?? appointment.LinkGoogleMeet;
            _appointmentRepo.Update(appointment);
            AppointmentEmailDTO emailModel = new AppointmentEmailDTO
            { 
                MenteeEmail = mentee.Email,
                MentorEmail = mentor.Email,
                Title = appointment.Title,
                Details = appointment.Detail,
                DateTime = $"{model.TimeStart} ngày {model.DateStart:dd/MM/yyyy}",
                LinkGoogleMeet = appointment.LinkGoogleMeet,
                VerifiedCode = appointment.VerifiedCode,
                MenteeName = mentee.Name
            };

            await _emailSender.SendEmailAppointment(emailModel, "updateAppointment");
            await _unitOfWork.CommitTransaction();
            return appointment;
        }

        public async Task<bool> VerifyAppointment(string code)
        {
            var findAppointment = await _appointmentRepo.FindAsync(_ => _.VerifiedCode == code);
            if (findAppointment == null) 
                return false;

            findAppointment.IsVerified = true;
            _appointmentRepo.Update(findAppointment);
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<AppointmentDTO> GetAppointmentById(string appointmentId)
        {
            var appointment = await _appointmentRepo.FindAsync(_ => _.Id == appointmentId);
            var appointmentDTO =  _map.Map<AppointmentDTO>(appointment);
            appointmentDTO.DateStart = appointment.DateStart.ToString("yyy-MM-dd");
            return appointmentDTO;
        }
    }
}
