using Mentore.Models.Base;

using System;
using System.Collections.Generic;

#nullable disable

namespace Mentore.Models
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
        }
#nullable enable

        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public System.Guid ActivationCode { get; set; }
        public System.Guid ResetPasswordCode { get; set; }
    }
}
