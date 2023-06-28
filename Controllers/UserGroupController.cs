using Mentore.Commons.CustomAttribute;
using Mentore.Models.DTOs.Requests;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mentore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Permission("MANAGE_PERMISSION")]
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserGroups()
        {
            var rs = await _userGroupService.GetUserGroups();
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok(rs.UserGroups);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserGroup([FromBody] string userGroupName)
        {
            var rs = await _userGroupService.AddUserGroup(userGroupName);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Add User Group " + userGroupName + " success !");
        }

        [HttpDelete("{userGroupId:int}")]
        public async Task<IActionResult> DeleteUserGroup(string userGroupId)
        {
            var rs = await _userGroupService.DeleteUserGroup(userGroupId);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok("Delete User Group " + userGroupId.ToString());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserGroup(UserGroupRequest request)
        {
            var rs = await _userGroupService.UpdateUserGroup(request);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok();
        }
    }
}