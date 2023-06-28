using API.Model.Entities;
using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Workshop : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int Attendees { get; set; }
        public int Participated { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
    }
}
