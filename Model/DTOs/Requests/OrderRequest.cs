using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentore.Models.DTOs.Requests
{
    public class OrderRequest
    {
        public int PaymentId { get; set; }
        public int ShopId { get; set; }
        public int Total { get; set; }
        public string BankCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

       // public List<CartRequest> Details { get; set; }
    }
}