using API.Model.DAL.Interfaces;
using API.Model.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class SpeakerWorkshopRepository : Repository<SpeakerWorkshop>, ISpeakerWorkshopRepository
    {
        public SpeakerWorkshopRepository(DbFactory dbFactory) : base(dbFactory)
        { }
    }
}
