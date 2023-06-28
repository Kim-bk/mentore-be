using System;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;

namespace Mentore.Services
{
    public interface IUserService
    {
        public Task<UserResponse> Login(LoginRequest req);

        public Task<UserResponse> Register(RegistRequest req, bool isCreateMentor = false);

        public Task<UserResponse> UpdateUser(UserRequest req, string idAccount);

        public Task<bool> CheckUserByActivationCode(Guid activationCode);

        public Task<UserResponse> ResetPassword(ResetPasswordRequest request);

        public Task<UserResponse> ForgotPassword(string userEmail);

        public Task<bool> GetUserByResetCode(Guid resetPassCode);

        public Task<UserResponse> FindById(string userId);

        public Task<bool> Logout(string userId);

    }
}