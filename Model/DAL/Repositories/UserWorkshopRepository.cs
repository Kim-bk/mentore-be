using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class UserWorkshopRepository : Repository<UserWorkshop>, IUserWorkshopRepository
    {
        public UserWorkshopRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
