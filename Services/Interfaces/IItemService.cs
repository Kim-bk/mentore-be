using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Mentore.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllItem();
        Task<ItemDTO> GetItemById(string IdItem);
        Task<ItemResponse> AddItem(ItemRequest req,string accountId);
        Task<ItemResponse> RemoveItemByItemId(string IdItem);
        Task<ItemResponse> UpdateItemByItemId(ItemRequest req,string accountId);
    }
}