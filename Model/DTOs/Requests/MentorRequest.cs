using Microsoft.AspNetCore.Http;
using System;

namespace API.Model.DTOs.Requests
{
    public class MentorRequest
    {
        public IFormFile File { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string LocationName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
        public string Job { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
    }
}
