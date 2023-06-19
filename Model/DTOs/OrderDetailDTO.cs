using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs
{
    public class OrderDetailDTO
    {
        public string Id { get; set; }
        public string Size { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public string ItemImg { get; set; }
        public int Price { get; set; }

    }
}