using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class MentorPositionRepository : Repository<MentorPosition>, IMentorPositionRepository
    {
        public MentorPositionRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
