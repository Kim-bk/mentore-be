using Mentore.Models;
using Mentore.Models.Base;
using System;

namespace API.Model.Entities
{
    public class Mentee : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string StudyAt { get; set; }
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
