using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.Entities;

namespace Mentore.Models.DAL.Repositories
{
    public class BankTypeRepository : Repository<BankType>, IBankTypeRepository
    {
        public BankTypeRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<BankType>> GetAllBanksType()
        {
            return await GetAll();
        }
    }
}