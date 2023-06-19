using System.Collections.Generic;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses.Base;
using Model.DTOs.Requests;

namespace Mentore.Services.Interfaces
{
    public interface IPermissionService
    {
        public Task<string> GetCredentials(string userId);
        public Task<GeneralResponse> AddCredential(CredentialRequest req);
        public Task<GeneralResponse> RemoveCredential(CredentialRequest req);

        public Task<bool> UpdateCredential(PermissionRequest req);
    }
}
