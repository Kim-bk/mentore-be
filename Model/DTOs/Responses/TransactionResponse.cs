using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.Responses
{
    public class TransactionResponse
    {
#nullable disable
        public string BillId { get; set; }
        public string CustomerName { get; set; }
        public string ShopName { get; set; }
        public string Money { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}