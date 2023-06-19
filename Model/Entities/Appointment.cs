using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Appointment : BaseEntity
    {
        public string Title { get; set; }
        public int MentorId { get; set; }
        public string accountId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
