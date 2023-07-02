using Microsoft.AspNetCore.Http;
using System;

namespace API.Model.DTOs.Requests
{
    public class WorkshopRequest
    {
        public string Id { get; set; }
        public IFormFile Image { get; set; }
        public string StartDate { get; set; }
        public string Time { get; set; }
        public bool IsOnline { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Attendees { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public string MentorIds { get; set; }
        public string Fields { get; set; }
    }
}
