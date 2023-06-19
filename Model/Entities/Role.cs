using Mentore.Models.Base;

#nullable disable

namespace Mentore.Models
{
    public partial class Role : BaseEntity
    {
        public string Name { get; set; }
        public string UserGroupId { get; set; }
    }
}
