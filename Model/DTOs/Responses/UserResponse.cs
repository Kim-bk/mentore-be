using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class UserResponse : GeneralResponse
    {
        public string AccountId { get; set; }
        public Account User { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}
