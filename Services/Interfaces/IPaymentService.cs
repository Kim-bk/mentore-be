using API.Model.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<(string, bool)> VNPayCheckOut(WorkshopRequest request, string userId, HttpContext context);
        public Task<bool> PaySuccess(IQueryCollection collections);
    }
}