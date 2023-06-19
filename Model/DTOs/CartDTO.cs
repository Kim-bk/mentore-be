using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs
{
    public class CartDTO
    {
        public string accountId { get; set; }
        public int OrderId {get; set;}
        public List<OrderDetailDTO> OrderDetails{ get; set; }
    }
}