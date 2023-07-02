using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Responses;
using Model.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Model.DTOs;
using DAL.Entities;

namespace Mentore.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<Post>> GetPosts();
        public Task<List<Post>> AcceptPost(string postId);
      //  public Task<List<CredentialResponse>> GetRolesOfUserGroup(string userGroup);

        public Task<UserResponse> Login(LoginRequest request);

        public Task<List<UserDTO>> GetUsers();

        public Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest request);
    }
}