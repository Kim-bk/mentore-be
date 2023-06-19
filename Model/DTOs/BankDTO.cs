using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Mentore.Models.DTOs
{
    public class BankDTO
    {
        public string Id { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }

#nullable enable
        public string? ExpiredDate { get; set; }
        public string? StartedDate { get; set; }
    }
}