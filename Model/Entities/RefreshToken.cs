using Mentore.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Mentore.Models
{
    public partial class RefreshToken 
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Account User { get; set; }
    }
}
