using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Mentore.Models.DTOs
{
    public class ItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public List<ImageDTO> Images { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}