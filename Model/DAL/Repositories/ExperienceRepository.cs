using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class ExperienceRepository : Repository<Experience>, IExperienceRepository
    {
        public ExperienceRepository(DbFactory dbFactory) : base(dbFactory)
        { }
    }
}
