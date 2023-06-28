using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepo;
        public LocationController(ILocationRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Location>> GetAll()
        {
            var rs = await _locationRepo.GetAll();
            return rs.OrderBy(_ => _.Name).ToList();
        }
    }
}
