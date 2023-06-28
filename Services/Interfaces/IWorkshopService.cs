using API.Model.DTOs;
using API.Model.DTOs.Requests;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IWorkshopService
    {
        public Task<List<WorkshopDTO>> GetAllWorkshops();
        public Task<Workshop> CreateWorkshop(WorkshopRequest model);
        public Task<bool> DeleteWorkshop(string workshopId);
        public Task<Workshop> UpdateWorkshop(WorkshopRequest model, string workshopId);
    }
}
