using System;

namespace API.Model.DTOs
{
    public class AppointmentDTO
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime DateStart { get; set; }
        public string MentorId { get; set; }
        public string TimeStart { get; set; }
    }
}
