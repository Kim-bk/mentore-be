using Mentore.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace Mentore.Models.DTOs.Responses
{
    public class CredentialResponse
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActivated { get; set; }
    }
}
