using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs
{
    public class StatisticalDTO
    {
        public int ItemId { get; set; }
        public string NameItem { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int CountSold { get; set; }
        public int Turnover {get; set;}
    }
}