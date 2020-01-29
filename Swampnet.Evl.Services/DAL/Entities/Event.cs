using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Services.DAL
{
    class EventEntity
    {
        public EventEntity()
        {
            Properties = new List<EventPropertyEntity>();
            CreatedOnUtc = DateTime.UtcNow;
        }


        public long Id { get; set; }
        public Guid Reference { get; set; }
        public string Summary { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        public int SourceId { get; set; }
        public SourceEntity Source { get; set; }

        public ICollection<EventPropertyEntity> Properties { get; set; }

        public DateTime TimestampUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }


    class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<EventEntity> Events { get; set; }
    }


    class SourceEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<EventEntity> Events { get; set; }
    }


    class EventPropertyEntity
    {
        public long Id { get; set; }

        public long EventId { get; set; }
        public EventEntity Event { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
