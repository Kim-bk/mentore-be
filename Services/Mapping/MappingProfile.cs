using API.Model.DTOs;
using AutoMapper;
using DAL.Entities;
using Mentore.Models;
using Mentore.Models.DTOs;
using Model.DTOs;

namespace Mentore.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, UserDTO>();
            CreateMap<UserGroup, UserGroupDTO>();
            CreateMap<Mentor, MentorDTO>().ReverseMap();
            CreateMap<Post, PostDTO>().ReverseMap();
        }
    }
}

