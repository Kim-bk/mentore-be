using DAL.Entities;
using System;
using System.Collections.Generic;

namespace API.Model.DTOs
{
    public class WorkshopDTO
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string StartDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Attendees { get; set; }
        public double Percentage { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public List<Mentor> Mentors { get; set; }
        public string Fields { get; set; }
        public string InvitationCode { get; set; }
    }
}
