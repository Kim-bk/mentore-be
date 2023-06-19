using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class MenteeRepository : Repository<Mentee>, IMenteeRepository
    {
        public MenteeRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
