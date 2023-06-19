using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models;
using Mentore.Models.DAL.Interfaces;

namespace Mentore.Models.DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
