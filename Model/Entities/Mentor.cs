using API.Model.Entities;
using Mentore.Models;
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
        public int LocationId { get; set; }
        public string Experience { get; set; }
        public string CV { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
        public string Address { get; set; }
        public virtual Account Account { get; set; }
        public virtual Location Location { get; set; } 
    }
}
