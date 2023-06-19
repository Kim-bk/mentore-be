using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class TokenResponse : GeneralResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ShopId { get; set; }
        public int Wallet { get; set; }
        public string UserId { get; set; }
    }
}