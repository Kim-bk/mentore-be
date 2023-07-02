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
        public string Time { get; set; }
        public string AccountId { get; set; }
        public string Avatar { get; set; }
        public string UserFullName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
