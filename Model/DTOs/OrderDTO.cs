using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs
{
    public class OrderDTO : GeneralResponse
    {
        public string Id { get; set; }
        public int StatusId { get; set; }
        public string BillId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public string NameOrder { get; set; }
        public string StatusName { get; set; }
        public string ShopName { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}