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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<AppointmentDTO> GetAppointmentById(string id)
        {
            return await _appointmentService.GetAppointmentById(id);
        }

        [HttpGet("user")]
        public async Task<List<AppointmentDTO>> GetUserAppointments()
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

            return Redirect("http://localhost:8080/book-success");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentDTO appointment)
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _appointmentService.CreateAppointment(appointment, userId);
            if (rs == null)
                return BadRequest("Khoảng thời gian đặt lịch đã bận. Xin vui lòng kiểm tra lại lịch cá nhân và của Mentor!");

            return Ok("Tạo lịch hẹn thành công. Vui lòng đợi Mentor xác nhận!");
        }

        [HttpPut("{id}")]
        public async Task<Appointment> UpdateAppointment(string id, AppointmentDTO appointment)
        {
            return await _appointmentService.UpdateAppointment(appointment, id);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAppointment(string id)
        {
            return await _appointmentService.DeleteAppointment(id);
        }
    }
}
