using Mentore.Models;
using Mentore.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Repositories
{
    public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<UserGroup>> GetMainUserGroup()
        {
            // admin - customer - shop
            return await GetQuery(ug => ug.Id == "User" || ug.Id == "Admin").ToListAsync();
        }
    }
}
