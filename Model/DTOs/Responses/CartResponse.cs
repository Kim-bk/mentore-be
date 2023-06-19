using System.Collections.Generic;
using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class CartResponse : GeneralResponse
    {
        public string ShopName {get; set;}
        public int ShopId { get; set; }
        public string ShopImage { get; set; }
        public List<OrderDetailDTO> OrderDetails {get;set;}
        
    }
}