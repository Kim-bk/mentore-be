using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models; 

namespace Mentore.Models.DTOs.Requests
{
    public class OrderDetailRequest
    {
        public string IdOrderDetail { get; set; }
        public int OrderId{ get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }

        // [Required]
        // public Item Item { get; set; }
    }
}