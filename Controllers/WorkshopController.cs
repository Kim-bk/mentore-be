using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Services.Interfaces;
using Castle.Core.Internal;
using DAL.Entities;
using Mentore.Commons.CustomAttribute;
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
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;
        public WorkshopController(IWorkshopService workshopService)
        {
            _workshopService = workshopService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<WorkshopDTO>> GetWorkShops()
        {
            return await _workshopService.GetAllWorkshops();

        }

        [HttpGet("mentee")]
        public async Task<List<WorkshopDTO>> GetMenteeWorkShops()
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _workshopService.GetMenteeWorkshops(userId);

        }

        [HttpGet("mentor")]
        public async Task<List<WorkshopDTO>> GetMentorWorkShops()
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _workshopService.GetMentorWorkshops(userId);
        }

        [Permission("CREATE_WORKSHOP")]
        [HttpGet("admin")]
        public async Task<List<WorkshopDTO>> GetAdminWorkShops()
        {
            return await _workshopService.GetAllWorkshops();
        }

        [Permission("CREATE_WORKSHOP")]
        [HttpPost]
        public async Task<Workshop> CreateWorkshop([FromForm] WorkshopRequest model)
        {
            return await _workshopService.CreateWorkshop(model);
        }

        [Permission("UPDATE_WORKSHOP")]
        [HttpPut("{id}")]
        public async Task<Workshop> UpdateWorkshop(string id, [FromForm] WorkshopRequest model)
        {
            return await _workshopService.UpdateWorkshop(model, id);
        }

        [Permission("CREATE_WORKSHOP")]
        [HttpGet("{id}")]
        public async Task<WorkshopDTO> GetWorkshop(string id)
        {
            return await _workshopService.GetWorkshop(id);
        }

        [Permission("DELETE_WORKSHOP")]
        [HttpDelete("{id}")]
        public async Task<List<WorkshopDTO>> DeleteWorkshop(string id)
        {
            return await _workshopService.DeleteWorkshop(id);
        }
    }
}
