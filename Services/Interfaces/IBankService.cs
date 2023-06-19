using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
using Mentore.Models.Entities;

namespace Mentore.Services.Interfaces
{
    public interface IBankService
    {
        Task<BankResponse> AddBank(BankRequest req, string idAccount);
        Task<BankResponse> UpdateBank(BankRequest req, string idAccount);
        Task<List<BankDTO>> GetBankById(string idAccount);
        Task<BankResponse> GetBanksByUser(string userId);
        Task<List<BankType>> GetBankType();
    }
}