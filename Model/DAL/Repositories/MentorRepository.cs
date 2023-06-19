using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class MentorRepository : Repository<Mentor>, IMentorRepository
    {
        public MentorRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
