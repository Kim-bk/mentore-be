using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentore.Models.DAL.Repositories
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Bank>> GetUserBanks(string accountId)
        {
            return await GetQuery(bk => bk.AccountId == accountId).ToListAsync();
        }
    }
}