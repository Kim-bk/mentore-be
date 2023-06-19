using Mentore.Models.Base;

namespace DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string Code { get; set; }
        public int MenteeId { get; set; }
        public int WorkShopId { get; set; }
    }
}
