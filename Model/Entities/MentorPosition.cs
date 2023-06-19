using API.Model.Entities;
using Mentore.Models.Base;

namespace DAL.Entities
{
    public class MentorPosition : BaseEntity
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int MentorFieldId { get; set; }
        public virtual EntityField MentorField { get; set; }
    }
}
