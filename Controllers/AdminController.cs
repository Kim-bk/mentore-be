using Mentore.Models.DTOs.Requests;
using Mentore.Commons.CustomAttribute;
using Mentore.Models.DTOs;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Model.DTOs;
using DAL.Entities;

namespace Mentore.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly IRoleService _roleService;

        public AdminController(IAdminService adminService, IAuthService authService, IRoleService roleService)
        {
            _adminService = adminService;
            _authService = authService;
            _roleService = roleService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var rs = await _adminService.Login(request);
            if (rs.IsSuccess)
            {
                // 1. Get list credentials of admin
                var listCredentials = await _roleService.GetCredentials(rs.User.Id);

                // 2. Authenticate admin
                var res = await _authService.Authenticate(rs.User, listCredentials);
                if (res.IsSuccess)
                    return Ok(res);
                else
                    return BadRequest(res.ErrorMessage);
            }

            return BadRequest(rs.ErrorMessage);
        }

        [Permission("ADMIN_GET_POST")]
        [HttpGet("post")]
        public async Task<List<Post>> GetPosts()
        {
            return await _adminService.GetPosts();
        }

        [Permission("ACCEPT_POST")]
        [HttpPost("post")]
        public async Task<List<Post>> AcceptPost(PostDTO post)
        {
            return await _adminService.AcceptPost(post.Id);
        }
    }
}