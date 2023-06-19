namespace Mentore.Models.DTOs.Requests
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public int Total { get; set; }
    }
}
