using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.Entities
{
    public class Event
    {
        public Event()
        {
            Properties = new List<EventProperty>();
            CreatedOnUtc = DateTime.UtcNow;
        }


        public long Id { get; set; }
        public Guid Reference { get; set; }
        public string Summary { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SourceId { get; set; }
        public Source Source { get; set; }

        public ICollection<EventProperty> Properties { get; set; }

        public DateTime TimestampUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }


    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Event> Events { get; set; }
    }


    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Event> Events { get; set; }
    }


    public class EventProperty
    {
        public long Id { get; set; }

        public long EventId { get; set; }
        public Event Event { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
