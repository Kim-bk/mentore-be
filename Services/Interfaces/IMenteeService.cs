using API.Model.DTOs;
using API.Model.Entities;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IMenteeService
    {
        public Task<MenteeDTO> GetMenteeData(string userId);
    }
}
