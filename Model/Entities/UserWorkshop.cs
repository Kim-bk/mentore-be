using Mentore.Models.Base;

namespace API.Model.Entities
{
    public class UserWorkshop : BaseEntity
    {
        public string MenteeId { get; set; }
        public string WorkshopId { get; set; }
        public bool IsActived { get; set; }
        public string InvitationCode { get; set; }
    }
}
