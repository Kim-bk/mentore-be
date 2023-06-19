using Mentore.Models.Base;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Mentore.Models
{
    public partial class Credential : BaseEntity
    {
        public string RoleId { get; set; }
        public string UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
