using Mentore.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Repositories
{
    public class CredentialRepository : Repository<Credential>, ICredentialRepository
    {
        public CredentialRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<string>> GetCredentialsByUserGroupId(string userGroupId)
        {
            return await GetQuery(cr => cr.UserGroupId == userGroupId && cr.IsActivated == true)
                         .Select(cr => cr.Role.Name)
                         .ToListAsync();
        }

        public async Task<List<Credential>> GetRolesOfUserGroup(string userGroupId)
        {
            return await GetQuery(cr => cr.UserGroupId == userGroupId && cr.IsActivated == true)
                        .ToListAsync();
        }

        public async Task<List<Credential>> GetRolesNotActivated(string userGroupId)
        {
            return await GetQuery(cr => cr.UserGroupId != userGroupId 
                        || (cr.UserGroupId == userGroupId && cr.IsActivated == false))
                        .ToListAsync();
        }
    }
}
