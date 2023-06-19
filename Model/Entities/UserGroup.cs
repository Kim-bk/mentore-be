using Mentore.Models.Base;
using System.Collections.Generic;

#nullable disable

namespace Mentore.Models
{
    public partial class UserGroup : BaseEntity
    {
        public UserGroup()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
