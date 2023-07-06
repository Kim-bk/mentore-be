using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Model.Entities;
using API.Services.Interfaces;
using AutoMapper;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
using Mentore.Services;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class MentorService : BaseService, IMentorService
    {
        private readonly IMentorRepository _mentorRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly IEntityFieldRepository _entityFieldRepo;
        private readonly IExperienceRepository _experienceRepo;
        private readonly IFieldRepository _fieldRepo;
        private readonly IUserService _userService;
        private readonly IMapper _map;
        private readonly UploadImageService _uploadImageService;
        public MentorService(
            IUnitOfWork unitOfWork,
            IMapperCustom mapper,
            IMentorRepository mentorRepo,
            IEntityFieldRepository entityFieldRepo,
            IFieldRepository fieldRepo,
            IExperienceRepository experienceRepo,
            IUserService userService,
            IMapper map,
            ILocationRepository locationRepo,
            UploadImageService uploadImageService) : base(unitOfWork, mapper)
        {
            _mentorRepo = mentorRepo;
            _locationRepo = locationRepo;
            _map = map;
            _userService = userService;
            _experienceRepo = experienceRepo;
            _fieldRepo = fieldRepo;
            _entityFieldRepo = entityFieldRepo;
            _uploadImageService = uploadImageService;
        }

        public async Task<bool> CreateMentor(MentorRequest model, string userId)
        {
            await _unitOfWork.BeginTransaction();
            var location = await _locationRepo.FindAsync(_ => _.Name == model.LocationName);

            // 1. Create mentor
            var mentor = new Mentor
            {
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                CurrentJob = model.CurrentJob,
                LocationId = location.Id,
                AccountId = userId,
                Avatar = model.File != null 
                    ? _uploadImageService.UploadFile(model.File) 
                    : "https://ui-avatars.com/api/?name=" + model.Name.ToLower().Trim()
            };

            await _mentorRepo.AddAsync(mentor);

            // 2. Add experience
            var expJobs = model.Job.Split('\n');
            var expCompanies = model.Company.Split('\n');
            var expYears = model.Year.Split('\n');
            var listExps = new List<Experience>();
            for (int i = 0; i < expJobs.Length; i++)
            {
                var experience = new Experience
                {
                    Job = expJobs[i].Replace("-", "").Trim(),
                    Company = expCompanies[i].Replace("-", "").Trim(),
                    Year = expYears[i].Replace("-", "").Trim(),
                    MentorId = mentor.Id
                };

                listExps.Add(experience);
            }

            await _experienceRepo.AddRangeAsync(listExps);

            // 3. Add field
            var fields = model.Fields.Split(',');
            for (int i = 0; i < fields.Length; i++)
            {
                var findField = await _fieldRepo.FindAsync(_ => _.Type == fields[i].Trim());
                var mentorField = new EntityField
                {
                    TableName = "Mentor",
                    TableId = mentor.Id,
                    FieldTypeId = findField.Id,
                };

                await _entityFieldRepo.AddAsync(mentorField);
            }

            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<MentorDTO> GetMentorById(string mentorId)
        {
            var findMentor = await _mentorRepo.FindAsync(_ => _.Id == mentorId);
            return await ProcessHandleMentorInfo(findMentor);
        }

        public async Task<MentorDTO> GetMentorByAccountId(string userId)
        {
            var findMentor = await _mentorRepo.FindAsync(_ => _.AccountId == userId);
            return await ProcessHandleMentorInfo(findMentor);
        }

        private async Task<MentorDTO> ProcessHandleMentorInfo(Mentor findMentor)
        {
            var mentor = _map.Map<MentorDTO>(findMentor);
            var location = await _locationRepo.FindAsync(_ => _.Id == findMentor.LocationId);
            mentor.LocationName = location.Name;
            mentor.BirthDate = findMentor.BirthDate.ToString("yyy-MM-dd");

            // Get mentor fields
            var mentorFieldIds = _entityFieldRepo.GetQuery(
                    _ => !_.IsDeleted
                    && _.TableName == "Mentor"
                    && _.TableId == mentor.Id).Select(_ => _.FieldTypeId).ToList();
            var mentorFieldNames = _fieldRepo.GetQuery(_ => mentorFieldIds.Contains(_.Id)).Select(_ => _.Type).ToList();
            foreach (var type in mentorFieldNames)
            {
                if (type != mentorFieldNames.LastOrDefault())
                    mentor.Fields += type + ", ";
                else
                    mentor.Fields += type;
            }

            // Get mentor experiences
            var exps = _experienceRepo.GetQuery(_ => _.MentorId == mentor.Id && !_.IsDeleted).ToList();
            mentor.Experiences = exps.OrderByDescending(_ => Convert.ToInt32(_.Year)).ToList();
            return mentor;
        }

        public async Task<List<MentorDTO>> GetMentors()
        {
            var listMentorsDto = new List<MentorDTO>();
            var mentors =  await _mentorRepo.GetAll();
            foreach (var item in mentors)
            {
                var mentor = _map.Map<MentorDTO>(item);
                var location = await _locationRepo.FindAsync(_ => _.Id == item.LocationId);
                mentor.LocationName = location.Name;

                // Get mentor field
                var mentorFieldIds = _entityFieldRepo.GetQuery(
                    _ => !_.IsDeleted
                    && _.TableName == "Mentor"
                    && _.TableId == mentor.Id).Select(_ => _.FieldTypeId).ToList();
                var mentorFieldNames = _fieldRepo.GetQuery(_ => mentorFieldIds.Contains(_.Id)).Select(_ => _.Type).ToList();
                foreach(var type in mentorFieldNames)
                {
                    if (type != mentorFieldNames.LastOrDefault())
                        mentor.Fields += type + ", ";
                    else
                        mentor.Fields += type;
                }

                // Get mentor experiences
                mentor.Experiences = _experienceRepo.GetQuery(_ => _.MentorId == mentor.Id && !_.IsDeleted).ToList();

                listMentorsDto.Add(mentor);
            }

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

        public async Task<UserResponse> Register(MentorRequest model)
        {
            // 1. Create account
            var account = new RegistRequest
            {
                Email = model.Email,
                Password = model.Password,
                ConfirmPassWord = model.Password,
            };

            var res = await _userService.Register(account, true);
            if (res.IsSuccess)
            {
                // 2. Add info to mentor table
                var isCreateSuccessMentor = await CreateMentor(model, res.AccountId);
                if (!isCreateSuccessMentor)
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không thể tạo thông tin cho Mentor. Vui lòng cập nhật lại ở trang thông tin cá nhân!"
                    };
            }

            return res;
        }

        public Task<bool> UpdateMentorInfo(MentorDTO model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteExperience(string experienceId)
        {
            var exp = await _experienceRepo.FindAsync(_ => _.Id == experienceId);
            exp.IsDeleted = true;
            _experienceRepo.Update(exp);
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<bool> CreateExperience(ExperienceDTO model, string userId)
        {
            var mentor = await _mentorRepo.FindAsync(_ => _.AccountId == userId);
            var exp = new Experience
            {
                Company = model.Company,
                Year = model.Year,
                Job = model.Job,
                MentorId = mentor.Id,
            };

            await _experienceRepo.AddAsync(exp);
            await _unitOfWork.CommitTransaction();
            return true;
        }
    }
}
