using Mentore.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace Mentore.Models.DTOs.Responses
{
    public class OrderResponse : GeneralResponse
    {
        public List<OrderDTO> Orders {get ;set; }
    }
}