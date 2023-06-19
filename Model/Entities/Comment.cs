using Mentore.Models.Base;
using System;

namespace DAL.Entities
{

    public class Comment : BaseEntity
    {
        public string accountId { get; set; }
        public string postId { get; set; }
        public string Content { get; set; }
    }
}
