using API.Model.DTOs;
using API.Model.DTOs.Requests;
using DAL.Entities;
using Mentore.Models.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IMentorService
    {
        public Task<UserResponse> Register(MentorRequest model);
        public Task<List<MentorDTO>> GetMentors();
        public Task<List<MentorDTO>> GetMentorsByFilter(string filter);
        public Task<MentorDTO> GetMentorById(string mentorId);
        public Task<bool> UpdateMentorInfo(MentorDTO model);
        public Task<bool> CreateMentor(MentorRequest model);
        public Task<List<Mentor>> ImportData(List<Mentor> listMentors);
    }
}
