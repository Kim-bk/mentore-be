using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Services.Interfaces;
using AutoMapper;
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

        public async Task<Appointment> CreateAppointment(AppointmentDTO model, string accountId)
        {
            if (ValidateDateTimeStart(model.DateStart, model.TimeStart))
                return null;

            var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == accountId);
           
            var appointment = new Appointment
            {
                Title = model.Title,
                Detail = model.Detail,
                DateStart = model.DateStart,
                MenteeId = mentee.Id,
                MentorId = model.MentorId,
                TimeStart = model.TimeStart,
                IsVerified = false,
                VerifiedCode = Guid.NewGuid().ToString(),
                LinkGoogleMeet = model.LinkGoogleMeet
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

            await _emailSender.SendEmailAppointment(emailModel);
            await _unitOfWork.CommitTransaction();

            return appointment;
        }

        private bool ValidateDateTimeStart(DateTime dateStart, string timeStart)
        {
            var existedAppointment = _appointmentRepo.GetQuery(_ => _.DateStart == dateStart && _.TimeStart == timeStart && !_.IsDeleted);
            if (existedAppointment == null) return true;

            return false;
        }    

        public async Task<bool> DeleteAppointment(string appointmentId)
        {
            var appointment = await _appointmentRepo.FindAsync(_ => _.Id == appointmentId);
            appointment.IsDeleted = true;
            _appointmentRepo.Update(appointment);

            // Send email cancel appointment
            var mentee = await _menteeRepo.FindAsync(_ => _.Id == appointment.MenteeId);
            var emailModel = new AppointmentEmailDTO
            {
                MenteeName = mentee.Name,
                Title = appointment.Title,
                DateTime = $"{appointment.TimeStart} ngày {appointment.DateStart:dd/MM/yyyy}",
                Details = appointment.Detail,
            };

            await _emailSender.SendEmailAppointment(emailModel, true);
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public List<Appointment> GetMentorAppointments(string mentorId)
        {
            return _appointmentRepo.GetQuery(_ => _.MentorId == mentorId && !_.IsDeleted && _.IsVerified).ToList();
        }

        public async Task<List<Appointment>> GetUserAppointment(string accountId)
        {
            var user = await _userRepo.FindAsync(_ => _.Id == accountId);
            if (user.UserGroupId == "MENTEE")
            {
                // Mentee appointmentss
                var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == accountId);
                return _appointmentRepo.GetQuery(_ => _.MenteeId == mentee.Id
                            && !_.IsDeleted && _.IsVerified 
                            && _.DateStart >= DateTime.Now)
                            .OrderByDescending(_ => _.DateStart).ToList();
            }

            // Mentor appointments
            var mentor = await _mentorRepo.FindAsync(_ => _.AccountId == accountId);
            return _appointmentRepo.GetQuery(_ => _.MentorId == mentor.Id 
                        && !_.IsDeleted && _.IsVerified 
                        && _.DateStart >= DateTime.Now)
                        .OrderByDescending(_ => _.DateStart).ToList();
        }

        public async Task<Appointment> UpdateAppointment(AppointmentDTO model, string appointmentId)
        {
            if (ValidateDateTimeStart(model.DateStart, model.TimeStart))
                return null;

            var appointment = await _appointmentRepo.FindAsync(_ => _.Id == appointmentId);
            appointment.Title = model.Title;
            appointment.Detail = model.Detail;
            appointment.DateStart = model.DateStart;
            appointment.TimeStart = model.TimeStart;

            _appointmentRepo.Update(appointment);
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
    }
}
