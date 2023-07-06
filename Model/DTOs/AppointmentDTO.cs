using System;

namespace API.Model.DTOs
{
    public class AppointmentDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string DateStart { get; set; }
        public string MentorId { get; set; }
        public string TimeStart { get; set; }
        public string LinkGoogleMeet { get; set; }
        public string VerifiedCode { get; set; }
        public bool IsVerified { get; set; }
        public string MenteeId { get; set; }
    }
}
