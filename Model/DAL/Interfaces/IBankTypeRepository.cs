using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.Entities;

namespace Mentore.Models.DAL.Interfaces
{
    public interface IBankTypeRepository : IRepository<BankType>
    {
        Task<List<BankType>> GetAllBanksType();
    }
}