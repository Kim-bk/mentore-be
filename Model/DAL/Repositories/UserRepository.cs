using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Repositories
{
    public class userRepo : Repository<Account>, IUserRepository
    {
        public userRepo(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await GetQuery(us => us.UserGroupId != "Admin" && us.IsActivated == true)
                        .ToListAsync();
        }
    }
}
