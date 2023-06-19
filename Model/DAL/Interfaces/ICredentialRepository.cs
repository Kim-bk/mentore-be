using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Interfaces
{
    public interface ICredentialRepository : IRepository<Credential>
    {
        Task<List<string>> GetCredentialsByUserGroupId(string userGroupId);

        Task<List<Credential>> GetRolesOfUserGroup(string userGroupId);

        Task<List<Credential>> GetRolesNotActivated(string userGroupId);
    }
}
