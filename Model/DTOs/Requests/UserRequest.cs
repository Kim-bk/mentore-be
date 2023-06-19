using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mentore.Models.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }       
        public string CV { get; set; }
        public DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string Experience { get; set; }
        public string CurrentJob { get; set; }
        public string StudyAt { get; set; }
    }
}