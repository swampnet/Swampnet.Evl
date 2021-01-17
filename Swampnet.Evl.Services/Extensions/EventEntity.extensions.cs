using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swampnet.Evl
{
    static class EventEntityExtensions
    {
        public static Event ToEvent(this EventEntity entity)
        {
            var e = new Event()
            {
                Id = entity.Reference,
                Summary = entity.Summary,
                TimestampUtc = entity.TimestampUtc,
                Category = (Category)Enum.Parse(typeof(Category), entity.Category.Name),
                Source = entity.Source.Name,
                Tags = entity.EventTags.Select(et => et.Tag.Name).ToList(),
                Properties = entity.Properties.Select(p => new Property(p.Name, p.Value, p.Category)).ToArray(),
                History = entity.History.Select(h => new EventHistory(h.Type, h.Details) { TimestampUtc = h.TimestampUtc }).ToList()
            };

            return e;
        }
    }
}
