using System;

namespace API.Model.DTOs.Requests
{
    public class MentorRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string Experience { get; set; }
        public string CV { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
        public string Address { get; set; }
    }
}
