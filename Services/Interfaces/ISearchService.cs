using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs;

namespace Mentore.Services.Interfaces
{
    public interface ISearchService
    {
        public Task<List<ItemDTO>> SearchItem(string searchContent);
    }
}
