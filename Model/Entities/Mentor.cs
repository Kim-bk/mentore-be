using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Mentor : BaseEntity
    {
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public string LocationId { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
    }
}
