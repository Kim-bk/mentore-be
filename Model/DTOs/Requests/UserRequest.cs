using Microsoft.AspNetCore.Http;
using System;

namespace Mentore.Models.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }       
        public string BirthDate { get; set; }
        public string CurrentJob { get; set; }
        public string LocationName { get; set; }
        public string StudyAt { get; set; }
        public string Fields { get; set; }
        public IFormFile Avatar { get; set; }
    }
}