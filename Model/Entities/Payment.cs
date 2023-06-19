using Mentore.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace Mentore.Models
{
    public partial class Payment : BaseEntity
    {
        public string Type { get; set; }
    }
}
