using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.Entities;
using API.Services.Interfaces;
using AutoMapper;
using Mentore.Models.DAL;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using System.Threading.Tasks;

namespace API.Services
{
    public class MenteeService : BaseService, IMenteeService
    {
        private readonly IMenteeRepository _menteeRepo;
        private readonly IMapper _map;
        public MenteeService(IMenteeRepository menteeRepo
            , IUnitOfWork unitOfWork
            , IMapper map
            , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _menteeRepo = menteeRepo;
            _map = map;
        }

        public async Task<MenteeDTO> GetMenteeData(string userId)
        {
            var mentee= await _menteeRepo.FindAsync(_ => _.AccountId == userId); 
            var result = _map.Map<MenteeDTO>(mentee);
            result.BirthDate = mentee.BirthDate.ToString("yyyy-MM-dd");
            return result;
        }
    }
}
