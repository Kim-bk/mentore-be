using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Services.Interfaces;
using AutoMapper;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class MentorService : BaseService, IMentorService
    {
        private readonly IMentorRepository _mentorRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly IEntityFieldRepository _entityFieldRepo;
        private readonly IFieldRepository _fieldRepo;
        private readonly IMapper _map;
        public MentorService(
            IUnitOfWork unitOfWork,
            IMapperCustom mapper,
            IMentorRepository mentorRepo,
            IEntityFieldRepository entityFieldRepo,
            IFieldRepository fieldRepo,
            IMapper map,
            ILocationRepository locationRepo) : base(unitOfWork, mapper)
        {
            _mentorRepo = mentorRepo;
            _locationRepo = locationRepo;
            _map = map;
        }

        public async Task<bool> CreateMentor(MentorDTO model)
        {
            model.Validate();

            await _unitOfWork.BeginTransaction();
            var location = await _locationRepo.FindAsync(_ => _.Id == model.LocationId);

            var mentor = new Mentor
            { 
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                CV = model.CV,
                Address = model.Address,
                BirthDate = model.BirthDate,
                Experience = model.Experience,
                CurrentJob = model.CurrentJob,
                Description = model.Description,
                Location = location,
            };

            await _mentorRepo.AddAsync(mentor);
            await _unitOfWork.CommitTransaction();

            return true;
        }

        public async Task<MentorDTO> GetMentorById(string mentorId)
        {
            var mentor = await _mentorRepo.FindAsync(_ => _.Id == mentorId);
            return _map.Map<MentorDTO>(mentor);
        }

        public async Task<List<MentorDTO>> GetMentors()
        {
            var listMentorsDto = new List<MentorDTO>();
            var mentors =  await _mentorRepo.GetAll();
            mentors.ForEach(mentor => listMentorsDto.Add(_map.Map<MentorDTO>(mentor)));
            return listMentorsDto;
        }

        public async Task<List<MentorDTO>> GetMentorsByFilter(string filter)
        {
            var listMentorsDto = new List<MentorDTO>();
            var mentors = new List<Mentor>();
            filter = filter.ToLower().Trim();
            switch (filter)
            {
                case "Name":
                    {
                        mentors =  _mentorRepo.GetQuery(_ => _.Name.ToLower()
                                                  .Contains(filter)).ToList();
                        mentors.ForEach(mentor => listMentorsDto.Add(_map.Map<MentorDTO>(mentor)));
                        break;
                    }
                case "Field":
                    {
                        var field = await _fieldRepo.FindAsync(_ => _.Type.ToLower().Contains(filter));
                        var mentorIds = _entityFieldRepo.GetQuery(_ => _.FieldTypeId == field.Id && _.TableName == "Mentor" && !_.IsDeleted)
                                                        .Select(_ => _.TableId);
                        mentors = _mentorRepo.GetQuery(_ => mentorIds.Contains(_.Id)).ToList();
                        break;
                    }
            }

            mentors.ForEach(mentor => listMentorsDto.Add(_map.Map<MentorDTO>(mentor)));
            return listMentorsDto;
        }

        public async Task<List<Mentor>> ImportData(List<Mentor> listMentors)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _mentorRepo.AddRangeAsync(listMentors);
                await _unitOfWork.CommitTransaction();
                return listMentors;
            }
            catch 
            {
                throw;
            }
        }

        public Task<bool> UpdateMentorInfo(MentorDTO model)
        {
            throw new System.NotImplementedException();
        }
    }
}
