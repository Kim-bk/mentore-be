using API.Model.DTOs;
using API.Model.Entities;
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
            CreateMap<Mentor, MentorDTO>()
                .ForMember(_ => _.BirthDate, opt => opt.Ignore())
                .ForMember(_ => _.Experiences, opt => opt.Ignore())
                .ForMember(_ => _.Fields, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Workshop, WorkshopDTO>().ReverseMap();
        }
    }
}

