using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses.Base;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IRoleService
    {
        Task<GeneralResponse> CreateRole(string roleName);
        Task<string> GetCredentials(string userId);
    }
}
