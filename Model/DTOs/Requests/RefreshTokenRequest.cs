using System.ComponentModel.DataAnnotations;

namespace Mentore.Models.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }
}
