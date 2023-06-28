using Mentore.Models.Base;

namespace API.Model.Entities
{
    public class SpeakerWorkshop : BaseEntity
    {
        public string MentorId { get; set; }
        public string WorkshopId { get; set; }
    }
}
