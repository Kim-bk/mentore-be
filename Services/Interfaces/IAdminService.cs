using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Responses;
using Model.DTOs;
using Model.DTOs.Requests;
using Model.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<CredentialResponse>> GetRolesOfUserGroup(string userGroup);

        public Task<UserResponse> Login(LoginRequest request);

        public Task<List<UserDTO>> GetUsers();

        public Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest request);

       // public Task<List<TransactionResponse>> GetTransactions();
    }
}