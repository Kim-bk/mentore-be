using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Repositories
{
    public interface IUserRepository : IRepository<Account>
    {
        public Task<List<Account>> GetAccounts();
    }
}
