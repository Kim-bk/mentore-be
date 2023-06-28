using System;

namespace API.Model.Entities
{
    public class Counter 
    {
        public string Id { get; set; }
        public long Value { get; set; }
        public Counter()
        {
            Id = Guid.NewGuid().ToString();
            Value = 0;
        }
    }
}
