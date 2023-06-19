using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Models.DAL.Interfaces
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        public Task<List<UserGroup>> GetMainUserGroup();
    }
}
