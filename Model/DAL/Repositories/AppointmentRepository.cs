using API.Model.DAL.Interfaces;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL;
using Mentore.Models;
using DAL.Entities;

namespace API.Model.DAL.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
