using Mentore.Models.DTOs.Requests;
using System.Threading.Tasks;

namespace Mentore.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> VNPayCheckOut(OrderRequest request, string userId);
        public Task<bool> CODCheckOut(OrderRequest request, string userId);
        public Task<bool> PaySuccess(string orderInfo);
    }
}