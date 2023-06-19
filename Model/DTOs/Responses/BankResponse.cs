using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class BankResponse : GeneralResponse
    {
        public List<BankDTO> UserBanks { get; set; }
    }
}