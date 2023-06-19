using Mentore.Models.Base;

#nullable disable
namespace Mentore.Models.Entities
{
    public partial class Bank : BaseEntity
    {
        public string BankNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountId { get; set; }
        public string BankTypeId { get; set; }
        public string StartedDate { get; set; }
        public string ExpiredDate { get; set; }
        public virtual Account Account { get; set; }
        public virtual BankType BankType { get; set; }
    }
}

