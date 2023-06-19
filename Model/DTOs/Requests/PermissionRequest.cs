using System.Collections.Generic;

namespace Model.DTOs.Requests
{
    public class PermissionRequest
    {
        public string UserGroupId { get; set; }
        public List<RoleDTO> Roles{ get; set; }
    }
}
