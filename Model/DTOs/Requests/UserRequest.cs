using Microsoft.AspNetCore.Http;
using System;

namespace Mentore.Models.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }       
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string Experience { get; set; }
        public string CurrentJob { get; set; }
        public string StudyAt { get; set; }
        public IFormFile Avatar { get; set; }
    }
}