using Mentore.Models;
using Mentore.Models.Base;

namespace DAL.Entities
{
    public class Participant :BaseEntity
    {
        public int ConversationId { get; set; }
        public string accountId { get; set; }
    }
}
