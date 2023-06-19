using Mentore.Models.Base;

namespace DAL.Entities
{
    public class Conversation : BaseEntity
    {
        public string LastMessage { get; set; }
    }
}
