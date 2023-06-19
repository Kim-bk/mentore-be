using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class RefreshTokenResponse : GeneralResponse
    {
        public RefreshToken RefreshToken { get; set; }
    }
}
