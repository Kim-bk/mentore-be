using System.Collections.Generic;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Responses;
using Model.DTOs.Responses;

namespace Mentore.Services
{
    public interface IShopService
    {
        public Task<UserResponse> Login(LoginRequest request);

        public Task<List<ShopDTO>> GetItemByShopId(string IdShop);

        public Task<List<ShopDTO>> GetCategories(string IdShop);

        public Task<ShopDTO> GetShop(string IdShop);

        public Task<List<OrderDTO>> GetOrder(string IdShop);

        public Task<List<TransactionResponse>> GetTransactions(int shopId);
        public Task<int> GetShopWallet(int shopId);
    }
}