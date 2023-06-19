using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
