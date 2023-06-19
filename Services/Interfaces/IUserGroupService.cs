using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
using Mentore.Models.DTOs.Responses.Base;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IUserGroupService
    {
        Task<UserGroupResponse> GetUserGroups();
        Task<GeneralResponse> AddUserGroup(string userGroupName);
        Task<GeneralResponse> DeleteUserGroup(string userGroupId);
        Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req);
    }
}
