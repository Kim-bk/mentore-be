using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs.Requests
{
    public class CredentialRequest
    {
        public string RoleId { get; set; }
        public string UserGroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
