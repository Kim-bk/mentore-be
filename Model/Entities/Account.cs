using Mentore.Models.Base;
using Mentore.Models.Entities;
using System;
using System.Collections.Generic;

#nullable disable

namespace Mentore.Models
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
            Banks = new HashSet<Bank>();
        }
#nullable enable

        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public int? Wallet { get; set; } 
        public DateTime DateCreated { get; set; }
        public string? UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public System.Guid ActivationCode { get; set; }
        public System.Guid ResetPasswordCode { get; set; }
       // public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
    }
}
