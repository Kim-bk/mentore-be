using Mentore.Models.DTOs.Requests;
using Mentore.Commons.CustomAttribute;
using Mentore.Models.DTOs;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.Requests;
using Model.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;

        public AdminController(IAdminService adminService, IAuthService authService
            , IPermissionService permissionService)
        {
            _adminService = adminService;
            _authService = authService;
            _permissionService = permissionService;
        }

        [Permission("MANAGE_PERMISSION")]
        [HttpGet("credentials/{userGroupId:int}")]
        public async Task<IActionResult> GetRolesOfUserGroup(string userGroupId)
        {
            var rs = await _adminService.GetRolesOfUserGroup(userGroupId);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var rs = await _adminService.Login(request);
            if (rs.IsSuccess)
            {
                // 1. Get list credentials of admin
                var listCredentials = await _permissionService.GetCredentials(rs.User.Id);

                // 2. Authenticate admin
                var res = await _authService.Authenticate(rs.User, listCredentials);
                if (res.IsSuccess)
                    return Ok(res);
                else
                    return BadRequest(res.ErrorMessage);
            }

            return BadRequest(rs.ErrorMessage);
        }

        [Permission("MANAGE_USER")]
        [HttpGet("user")]
        public async Task<List<UserDTO>> GetUsers()
        {
            return await _adminService.GetUsers();
        }

        [Permission("MANAGE_USER")]
        [HttpPut("user")]
        public async Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest req)
        {
            return await _adminService.UpdateUserGroupOfUser(req);
        }

/*        [HttpGet("transaction")]
        public async Task<List<TransactionResponse>> GetTransactions()
        {
            return await _adminService.GetTransactions();
        }*/
    }
}