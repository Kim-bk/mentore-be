using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
// using Microsoft.AspNetCore.Mvc;

namespace Mentore.Services.Interfaces
{
    public interface ICartService 
    {
        Task<List<CartResponse>> GetCartById(string idAccount);
        // List<OrderDetailDTO> GetOrderDetail(string IdOrder);
    }
}