using System;
using System.ComponentModel.DataAnnotations;

namespace Mentore.Models.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            IsDeleted = false;
            UpdatedAt = DateTime.Now;
        }
    }
}
