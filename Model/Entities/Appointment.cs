using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Appointment : BaseEntity
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string MentorId { get; set; }
        public string AccountId { get; set; }
        public DateTime DateStart { get; set; }
        public string TimeStart { get; set; }
    }
}
