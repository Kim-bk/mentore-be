using API.Model.DTOs;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<List<MentorDTO>> GetMentors();
        public Task<List<MentorDTO>> GetMentorsByFilter(string filter);
        public Task<MentorDTO> GetMentorById(string mentorId);
        public Task<bool> UpdateMentorInfo(MentorDTO model);
        public Task<bool> CreateMentor(MentorDTO model);
        public Task<List<Mentor>> ImportData(List<Mentor> listMentors);
    }
}
