using API.Model.Entities;
using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Appointment : BaseEntity
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime DateStart { get; set; }
        public string TimeStart { get; set; }
        public string LinkGoogleMeet { get; set; }
        public string VerifiedCode { get; set; }
        public bool IsVerified { get; set; }
        public string MentorId { get; set; }
        public string MenteeId { get; set; }
    }
}
