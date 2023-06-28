using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Services.Interfaces;
using AutoMapper;
using DAL.Entities;
using Mentore.Models.DAL;
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
        private readonly IMapper _map;
        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapperCustom mapper,
            IAppointmentRepository appointmentRepo,
            IMapper map) : base(unitOfWork, mapper)
        {
            _map = map;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Appointment> CreateAppointment(AppointmentDTO model, string accountId)
        {
            if (ValidateDateTimeStart(model.DateStart, model.TimeStart))
                return null;

            var appointment = new Appointment
            {
                Title = model.Title,
                Detail = model.Detail,
                DateStart = model.DateStart,
                AccountId = accountId,
                MentorId = model.MentorId,
                TimeStart = model.TimeStart
            };

            await _appointmentRepo.AddAsync(appointment);
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
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public List<Appointment> GetMentorAppointments(string mentorId)
        {
            return _appointmentRepo.GetQuery(_ => _.MentorId == mentorId && !_.IsDeleted).ToList();
        }

        public List<Appointment> GetUserAppointment(string accountId)
        {
            return _appointmentRepo.GetQuery(_ => _.AccountId == accountId && !_.IsDeleted).ToList();
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
    }
}
