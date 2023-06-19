using Mentore.Models.Base;

namespace Mentore.Models.Entities
{
    public class BankType : BaseEntity
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
