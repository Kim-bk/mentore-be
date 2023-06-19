using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class FieldRepository : Repository<Field>, IFieldRepository
    {
        public FieldRepository(DbFactory dbFactory) : base(dbFactory)
        { }
    }
}
