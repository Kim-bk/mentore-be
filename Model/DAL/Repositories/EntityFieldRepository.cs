using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class EntityFieldRepository : Repository<EntityField>, IEntityFieldRepository
    {
        public EntityFieldRepository(DbFactory dbFactory) : base(dbFactory)
        { }
    }
}
