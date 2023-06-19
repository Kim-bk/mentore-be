using Mentore.Models.DTOs.Responses.Base;
using Model.DTOs;
using System.Collections.Generic;

namespace Mentore.Models.DTOs.Responses
{
    public class UserGroupResponse : GeneralResponse
    {
        public List<UserGroupDTO> UserGroups { get; set; }
    }
}
