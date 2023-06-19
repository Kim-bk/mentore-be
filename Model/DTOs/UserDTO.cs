using System;

namespace Mentore.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
    }
}
