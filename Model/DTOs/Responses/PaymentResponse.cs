namespace API.Model.DTOs.Responses
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string OrderInfo { get; set; }
        public string UserWorkshopId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public string Amount { get; set; }
    }
}
