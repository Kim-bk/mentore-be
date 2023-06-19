using System.Threading.Tasks;
using Mentore.Commons.CustomAttribute;
using Mentore.Models.DTOs.Requests;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.Requests;

namespace Mentore.Controllers
{
    [Authorize]
    [Permission("MANAGE_PERMISSION")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("credential")]
        // api/permission/credential
        public async Task<IActionResult> AddCredential(CredentialRequest req)
        {
            var rs = await _permissionService.AddCredential(req);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }
       
        [HttpDelete("credential")]
        // api/permission/credential
        public async Task<IActionResult> RemoveCredential(CredentialRequest req)
        {
            var rs = await _permissionService.RemoveCredential(req);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("credential")]
        // api/permission/credential
        public async Task<bool> UpdateCredential([FromBody] PermissionRequest req)
        {
            try
            {
                return await _permissionService.UpdateCredential(req);
            }
            catch
            {
                throw;
            }
        }
    }
}
