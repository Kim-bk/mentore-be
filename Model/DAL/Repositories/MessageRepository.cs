using API.Model.DAL.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;

namespace API.Model.DAL.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
