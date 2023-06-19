using Mentore.Models;
using Mentore.Models.DTOs.Responses;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> Authenticate(Account usser, string listCredentials, string userGroup = "");
    }
}