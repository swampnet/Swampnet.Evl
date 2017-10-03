using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Linq;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.DAL.InMemory
{
    static partial class Convert
    {
        #region Event

        /// <summary>
        /// Convert an API Event to an InternalEvent
        /// </summary>
        internal static InternalEvent ToInternalEvent(Event evt)
        {
            return evt == null
                ? null
                : new InternalEvent()
                {
                    Category = evt.Category.ToString(),
                    Summary = evt.Summary,
                    TimestampUtc = evt.TimestampUtc,
                    LastUpdatedUtc = evt.LastUpdatedUtc.HasValue ? evt.LastUpdatedUtc.Value : evt.TimestampUtc,
                    Properties = evt.Properties?.Select(p => ToInternalProperty(p)).ToList()

                };
        }


        /// <summary>
        /// Convert an API IProperty to an InternalProperty
        /// </summary>
        internal static InternalProperty ToInternalProperty(IProperty property)
        {
            return new InternalProperty()
            {
                Category = property.Category,
                Name = property.Name,
                Value = property.Value
            };
        }


        /// <summary>
        /// Convert an InternalEvent to an API Event 
        /// </summary>
        internal static Event ToEvent(InternalEvent evt)
        {
            return evt == null 
                ? null 
                : new Event()
                {
                    Id = evt.Id,
                    Category = Enum.Parse<EventCategory>(evt.Category, true),
                    Summary = evt.Summary,
                    TimestampUtc = evt.TimestampUtc,
                    LastUpdatedUtc = evt.LastUpdatedUtc,
                    Properties = evt.Properties?.Select(p => ToProperty(p)).ToList()
                };
        }

        /// <summary>
        /// Convert an InternalEvent to an EventSummary
        /// </summary>
        internal static EventSummary ToEventSummary(InternalEvent evt)
        {
            return new EventSummary()
            {
                Id = evt.Id,
                Category = Enum.Parse<EventCategory>(evt.Category,true),
                Summary = evt.Summary,
                TimestampUtc = evt.TimestampUtc
            };
        }

        /// <summary>
        /// Convert an IProperty to a Property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        internal static Property ToProperty(IProperty property)
        {
            return new Property()
            {
                Category = property.Category,
                Name = property.Name,
                Value = property.Value
            };
        }
        #endregion
    }
}
