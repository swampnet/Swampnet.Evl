using System;
using System.Linq;
using Swampnet.Evl.Client;
using System.Collections.Generic;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    static partial class Convert
    {
        #region Event

        /// <summary>
        /// Convert an API Event to an InternalEvent
        /// </summary>
        internal static InternalEvent ToInternalEvent(Event evt, EventContext context)
        {
            InternalEvent e = null;

            if (evt != null)
            {
                e = new InternalEvent()
                {
                    Category = evt.Category.ToString(),
                    Summary = evt.Summary,
                    TimestampUtc = evt.TimestampUtc,
                    LastUpdatedUtc = evt.LastUpdatedUtc.HasValue ? evt.LastUpdatedUtc.Value : evt.TimestampUtc,
                    Source = evt.Source,
                    SourceVersion = evt.SourceVersion                    
                };

                e.AddProperties(evt.Properties);
                e.AddTags(context, evt.Tags);
            }

            return e;
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
                    Properties = evt.InternalEventProperties == null
                        ? null
                        : evt.InternalEventProperties.Select(p => Convert.ToProperty(p.Property)).ToList(),
                    Source = evt.Source,
                    SourceVersion = evt.SourceVersion,
                    Tags = evt.InternalEventTags == null
                        ? null
                        : evt.InternalEventTags.Select(t => t.Tag.Name).ToList()
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
                TimestampUtc = evt.TimestampUtc,
                Source = evt.Source
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


        // @TODO: Possibly better as .AddTags and just adding tag data to event directly
        private static List<InternalEventTags> CreateTags(string[] tags, InternalEvent evt, EventContext context)
        {
            List<InternalEventTags> links = null;

            if(tags != null && tags.Any())
            {
                links = new List<InternalEventTags>();
                foreach(var tag in tags)
                {
                    var link = new InternalEventTags();
                    link.Event = evt;

                    var t = context.Tags.FirstOrDefault(x => x.Name == tag); // .First() - it *is* possible to have multiple tags with same name (due to syncronisation, or lack of lol!)
                    if(t == null)
                    {
                        t = new InternalTag()
                        {
                            Name = tag
                        };
                        context.Tags.Add(t);
                    }
                    link.Tag = t;
                    links.Add(link);
                }
            }

            return links;
        }

    }
}
