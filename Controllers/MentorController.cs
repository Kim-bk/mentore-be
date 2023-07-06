using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Model.DTOs;
using Mentore.Models.DTOs.Responses;
using API.Model.DTOs.Requests;
using System.Security.Claims;
using System;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorService _mentorService;

        public MentorController(IMentorService mentorService)
        {
            _mentorService = mentorService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<UserResponse> Register([FromForm] MentorRequest model)
        {
            return await _mentorService.Register(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<MentorDTO>> GetMentors()
        {
           return await _mentorService.GetMentors();
        }

        [AllowAnonymous]
        [HttpGet("profile/{id}")]
        public async Task<MentorDTO> GetMentorById(string id)
        {
            return await _mentorService.GetMentorById(id);
        }

        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<List<MentorDTO>> GetMentorsByFilter([FromQuery] string filter)
        {
            return await _mentorService.GetMentorsByFilter(filter);
        }


        [HttpDelete("experience/{id}")]
        public async Task<bool> DeleteExperience(string id)
        {
            return await _mentorService.DeleteExperience(id);
        }

        [HttpPost("experience")]
        public async Task<bool> CreateExperience(ExperienceDTO model)
        {
            string userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _mentorService.CreateExperience(model, userId);
        }
    }
}
