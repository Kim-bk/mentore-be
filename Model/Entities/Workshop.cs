using API.Model.Entities;
using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Workshop : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Anttendees { get; set; }
        public int Price { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; } 
    }
}
