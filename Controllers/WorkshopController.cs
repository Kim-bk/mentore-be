using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Services.Interfaces;
using Castle.Core.Internal;
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
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId.IsNullOrEmpty())
                return await _workshopService.GetAllWorkshops();

            return null;
           // return await _workshopService.GetUserWorkshops(userId);
        }

        [HttpPost]
        public async Task<Workshop> CreateWorkshop([FromForm] WorkshopRequest model)
        {
            return await _workshopService.CreateWorkshop(model);
        }

        [HttpPut("id")]
        public async Task<Workshop> UpdateWorkshop([FromForm] WorkshopRequest model, [FromQuery] string id)
        {
            return await _workshopService.UpdateWorkshop(model, id);
        }
    }
}
