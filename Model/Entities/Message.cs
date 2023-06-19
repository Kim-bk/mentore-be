using Mentore.Models;
using Mentore.Models.Base;

namespace DAL.Entities
{
    public class Message : BaseEntity
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}
