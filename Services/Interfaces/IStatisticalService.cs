using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;

namespace Mentore.Services
{
    public interface IStatisticalService
    {
        Task<List<StatisticalDTO>> ListItemsSold(string IdShop);
        Task<List<StatisticalDTO>> ListItemsSoldByDate(string IdShop, string dateTime);
        Task<IntervalResponse> ListItemSoldBy7Days(string type,int shopId);
        Task<IntervalResponse> ListIntervalCancelOrder(string type,string userId);
        Task<IntervalResponse> CountOrders(string type,string userId);
        Task<IntervalResponse> CountOrdersCancel(string type,string userId);
    }
}