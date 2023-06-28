using Mentore.Models.Base;

namespace API.Model.Entities
{
    public class Experience : BaseEntity
    {
        public string MentorId { get; set; }
        public string Job { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
    }
}
