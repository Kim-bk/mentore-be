using Mentore.Commons.CustomAttribute;
using Mentore.Models.DTOs.Requests;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mentore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        // api/role
        public async Task<IActionResult> CreateRole()
        {
            var rs = await _roleService.CreateRole("random");
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Creata role success !");
        }
    }
}
