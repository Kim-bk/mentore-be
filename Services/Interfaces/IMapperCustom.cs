using System.Collections.Generic;
using Mentore.Models;
using Mentore.Models.DTOs;
using Model.DTOs;

namespace Mentore.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<UserDTO> MapUsers(List<Account> users);
        List<UserGroupDTO> MapUserGroups(List<UserGroup> orders);
    }
}

