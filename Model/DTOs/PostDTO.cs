using System;

namespace API.Model.DTOs
{
    public class PostDTO
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileUrl { get; set; }
        public bool IsContainVideo { get; set; }
        public string VideoUrl { get; set; }
        public int Like { get; set; }
        public int Heart { get; set; }
        public string Time { get; set; }
        public string AccountId { get; set; }
    }
}
