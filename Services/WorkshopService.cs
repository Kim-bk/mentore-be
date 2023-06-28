using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Model.Entities;
using API.Services.Interfaces;
using AutoMapper;
using Castle.Core.Internal;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class WorkshopService : BaseService, IWorkshopService
    {
        private readonly IWorkshopRepository _workshopRepo;
        private readonly ISpeakerWorkshopRepository _speakerWorkshopRepo;
        private readonly IEntityFieldRepository _entityFieldRepo;
        private readonly IFieldRepository _fieldRepo;
        private readonly IMentorRepository _mentorRepo;
        private readonly UploadImageService _uploadImageService;
        private readonly IRepository<Counter> _counterRepo;
        private readonly IMapper _map;

        private static long counter;

        public WorkshopService(IUnitOfWork unitOfWork, IMapperCustom mapper
            , IWorkshopRepository workshopRepo
            , ISpeakerWorkshopRepository speakerWorkshopRepo
            , IMentorRepository mentorRepo
            , IFieldRepository fieldRepo
            , IEntityFieldRepository entityFieldRepo
            , UploadImageService uploadImageService
            , IRepository<Counter> counterRepo
            , IMapper map) : base(unitOfWork, mapper)
        {
            _workshopRepo = workshopRepo;
            _speakerWorkshopRepo = speakerWorkshopRepo;
            _mentorRepo = mentorRepo;
            _entityFieldRepo = entityFieldRepo;
            _fieldRepo = fieldRepo;
            _uploadImageService = uploadImageService;
            _counterRepo = counterRepo;
            _map = map;
        }

        public async Task<Workshop> CreateWorkshop(WorkshopRequest model)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var workshop = new Workshop
                {
                    Title = model.Title.ToUpper(),
                    StartDate = model.StartDate,
                    Time = model.Time,
                    Content = model.Content,
                    Attendees = model.Attendees,
                    Price = model.Price,
                    Location = model.Location,
                    Participated = 0
                };

                // Add fields for workshop
                var fields = model.Fields.Split(',');
                var listWorkshopFields = new List<EntityField>();
                for (int i = 0; i < fields.Length; i++)
                {
                    var findField = await _fieldRepo.FindAsync(_ => _.Type == fields[i].Trim());
                    var workshopField = new EntityField
                    {
                        TableName = "Workshop",
                        TableId = workshop.Id,
                        FieldTypeId = findField.Id,
                    };

                    listWorkshopFields.Add(workshopField);
                }

                await _entityFieldRepo.AddRangeAsync(listWorkshopFields);

                // Add speakers
                var mentorIds = model.MentorIds.Split(',');
                var listSpeakers = new List<SpeakerWorkshop>();
                for (int i = 0; i < fields.Length; i++)
                {
                    var findMentor = await _mentorRepo.FindAsync(_ => _.Id == mentorIds[i].Trim());
                    var speakerMentor = new SpeakerWorkshop
                    { 
                        MentorId = findMentor.Id,
                        WorkshopId = workshop.Id,
                    };

                    listSpeakers.Add(speakerMentor);
                }

                await _speakerWorkshopRepo.AddRangeAsync(listSpeakers);

                // Upload image
                if (model.Image != null)
                    workshop.Image = _uploadImageService.UploadFile(model.Image);

                await _workshopRepo.AddAsync(workshop);
                await _unitOfWork.CommitTransaction();

                return workshop;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<bool> DeleteWorkshop(string workshopId)
        {
            var workshop = await _workshopRepo.FindAsync(_ => _.Id == workshopId);
            workshop.IsDeleted = true;
            return true;
        }

        public async Task<List<WorkshopDTO>> GetAllWorkshops()
        {
            var workshops = await _workshopRepo.GetAll();
            var listWorkshopDTO = new List<WorkshopDTO>();
          
            foreach (var workshop in workshops.OrderByDescending(_ => _.CreatedAt))
            {
                var workshopDTO = _map.Map<WorkshopDTO>(workshop);
                workshopDTO.Percentage = Math.Round((double)workshop.Participated / (double)workshop.Attendees * 100, 2);
                workshopDTO.StartDate = DateTime
                    .ParseExact(workshop.StartDate.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                    .ToString("dd/MM/yyyy");

                var speakerIds = _speakerWorkshopRepo.GetQuery(_ => _.WorkshopId == workshop.Id).Select(_ => _.MentorId).ToList();

                // Get worshop speakers 
                var speakers = _mentorRepo.GetQuery(_ => speakerIds.Contains(_.Id)).ToList();
                workshopDTO.Mentors = speakers;

                // Get workshop fields
                var workShopFields = _entityFieldRepo.GetQuery(
                        _ => !_.IsDeleted
                        && _.TableName == "Workshop"
                        && _.TableId == workshop.Id).Select(_ => _.FieldTypeId).ToList();

                var workshopFieldNames = _fieldRepo.GetQuery(_ => workShopFields.Contains(_.Id)).Select(_ => _.Type).ToList();
                foreach (var type in workshopFieldNames)
                {
                    if (type != workshopFieldNames.LastOrDefault())
                        workshopDTO.Fields += type + ", ";
                    else
                        workshopDTO.Fields += type;
                }

              

                listWorkshopDTO.Add(workshopDTO);
            }

            return listWorkshopDTO;
        }

        public async Task<Workshop> UpdateWorkshop(WorkshopRequest model, string workshopId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var workshop = await _workshopRepo.FindAsync(_ => _.Id == workshopId);
                workshop.Title = model.Title.ToUpper();
                workshop.StartDate = model.StartDate;
                workshop.Time = model.Time;
                workshop.Content = model.Content;
                workshop.Attendees = model.Attendees;
                workshop.Price = model.Price;
                workshop.Location = model.Location;
            
                if (!model.Fields.IsNullOrEmpty())
                {
                    // Add fields for workshop
                    var fields = model.Fields.Split(',');
                    var listWorkshopFields = new List<EntityField>();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        var findField = await _fieldRepo.FindAsync(_ => _.Type == fields[i].Trim());
                        var workshopField = new EntityField
                        {
                            TableName = "Workshop",
                            TableId = workshop.Id,
                            FieldTypeId = findField.Id,
                        };

                        listWorkshopFields.Add(workshopField);
                    }

                    await _entityFieldRepo.AddRangeAsync(listWorkshopFields);
                }

                // Add speakers
                if (!model.MentorIds.IsNullOrEmpty())
                {
                    var mentorIds = model.MentorIds.Split(',');
                    var listSpeakers = new List<SpeakerWorkshop>();
                    for (int i = 0; i < mentorIds.Length; i++)
                    {
                        var findMentor = await _mentorRepo.FindAsync(_ => _.Id == mentorIds[i].Trim());
                        var speakerMentor = new SpeakerWorkshop
                        {
                            MentorId = findMentor.Id,
                            WorkshopId = workshop.Id,
                        };

                        listSpeakers.Add(speakerMentor);
                    }

                    await _speakerWorkshopRepo.AddRangeAsync(listSpeakers);
                }    

                // Upload image
                if (model.Image != null)
                    workshop.Image = _uploadImageService.UploadFile(model.Image);

                _workshopRepo.Update(workshop);
                await _unitOfWork.CommitTransaction();

                return workshop;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
