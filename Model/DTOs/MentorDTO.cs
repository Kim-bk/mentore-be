using API.Model.Entities;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace API.Model.DTOs
{
    public class MentorDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string LocationName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentJob { get; set; }
        public string Avatar { get; set; }
        public List<Experience> Experiences { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Fields {get; set; }
    }

}
