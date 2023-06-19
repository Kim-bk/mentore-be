using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mentore.Services.Interfaces;
using Mentore.Models;
using Mentore.Models.DTOs;
using Model.DTOs;

namespace Mentore.Services.Mapping
{
    public class Mapper : IMapperCustom
    {
        private readonly IMapper _autoMapper;
        public Mapper(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public List<UserDTO> MapUsers(List<Account> users)
        {
            var listUser = new List<UserDTO>();
            foreach(var u in users)
            {
                var user = new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    DateCreated = u.DateCreated,
                    Name = u.Name,
                    UserGroupId = u.UserGroupId,
                };
                listUser.Add(user);
            }    
            return listUser;
        }

        public List<UserGroupDTO> MapUserGroups(List<UserGroup> userGroups)
        {
            return _autoMapper.Map<List<UserGroup>, List<UserGroupDTO>>(userGroups);
        }
    }
}
