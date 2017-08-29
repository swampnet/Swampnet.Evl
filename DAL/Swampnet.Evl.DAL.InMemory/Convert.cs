using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.DAL.InMemory
{
    static class Convert
    {
        internal static InternalEvent ToInternalEvent(Event evt)
        {
            return evt == null
                ? null
                : new InternalEvent()
                {
                    Category = evt.Category,
                    Summary = evt.Summary,
                    TimestampUtc = evt.TimestampUtc,
                    Properties = evt.Properties?.Select(p => ToInternalProperty(p)).ToList()
                };
        }


        internal static InternalProperty ToInternalProperty(IProperty property)
        {
            return new InternalProperty()
            {
                Category = property.Category,
                Name = property.Name,
                Value = property.Value
            };
        }

        internal static Event ToEvent(InternalEvent evt)
        {
            return evt == null 
                ? null 
                : new Event()
                {
                    Id = evt.Id,
                    Category = evt.Category,
                    Summary = evt.Summary,
                    TimestampUtc = evt.TimestampUtc,
                    Properties = evt.Properties?.Select(p => ToProperty(p)).ToList()
                };
        }

        internal static EventSummary ToEventSummary(InternalEvent evt)
        {
            return new EventSummary()
            {
                Id = evt.Id,
                Category = evt.Category,
                Summary = evt.Summary,
                TimestampUtc = evt.TimestampUtc
            };
        }

        internal static Property ToProperty(IProperty property)
        {
            return new Property()
            {
                Category = property.Category,
                Name = property.Name,
                Value = property.Value
            };
        }
    }
}
