using API.Model.DTOs;
using API.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [AllowAnonymous]
        [HttpGet("mentor/{id}")]
        public List<Appointment> GetMentorAppointments(string id)
        {
            return _appointmentService.GetMentorAppointments(id);
        }

        [HttpGet("user")]
        public async Task<List<Appointment>> GetUserAppointments()
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _appointmentService.GetUserAppointment(userId);
        }

        [AllowAnonymous]
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyAppointment([FromQuery] string code)
        {
            var verifyAppointment = await _appointmentService.VerifyAppointment(code);
            if (!verifyAppointment) 
                return BadRequest("Xác nhận lịch hẹn thất bại!");

            return Ok("Bạn đã xác nhận lịch hẹn thành công! Vào tài khoản trên Mentore để kiểm tra lịch hẹn");
        }

        [HttpPost]
        public async Task<Appointment> CreateAppointment(AppointmentDTO appointment)
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _appointmentService.CreateAppointment(appointment, userId);
        }

        [HttpPut("id")]
        public async Task<Appointment> UpdateAppointment([FromQuery] string id, AppointmentDTO appointment)
        {
            return await _appointmentService.CreateAppointment(appointment, id);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAppointment(string id)
        {
            return await _appointmentService.DeleteAppointment(id);
        }
    }
}
