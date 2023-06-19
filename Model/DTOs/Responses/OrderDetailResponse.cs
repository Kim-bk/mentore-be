using Mentore.Models.DTOs.Responses.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs.Responses
{
    public class OrderDetailResponse : GeneralResponse
    {
        public List<OrderDetailDTO> OrderDetail;
    }
}
