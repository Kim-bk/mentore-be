using Mentore.Models.Base;
using System;

namespace DAL.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileUrl { get; set; }
        public bool IsContainVideo { get; set; }
        public string VideoUrl { get; set; }
        public int Like { get; set; }
        public int Heart { get; set; }
        public string AccountId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
