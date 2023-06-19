using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Model.DTOs;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : Controller
    {
        private readonly IMentorService _mentorService;

        public MentorController(IMentorService mentorService)
        {
            _mentorService = mentorService;
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
    }
}
