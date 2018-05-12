using System;
using System.Linq;
using Swampnet.Evl.Client;
using System.Collections.Generic;
using Swampnet.Evl.DAL.MSSQL.Entities;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Common;

namespace Swampnet.Evl.DAL.MSSQL
{
    /// <summary>
    /// Events
    /// </summary>
    static partial class Convert
    {
        /// <summary>
        /// Convert an API Event to an InternalEvent
        /// </summary>
        internal static InternalEvent ToEvent(Organisation org, EventDetails source, EvlContext context)
        {
            if(org == null)
            {
                throw new ArgumentNullException("org");
            }

            InternalEvent e = null;

            if (source != null)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                e = new InternalEvent()
                {
                    OrganisationId = org.Id,
                    Category = source.Category.ToString(),
                    Summary = source.Summary,
                    TimestampUtc = source.TimestampUtc,
                    ModifiedOnUtc = source.LastUpdatedUtc.HasValue ? source.LastUpdatedUtc.Value : source.TimestampUtc,
                    Source = source.Source.Truncate(2000),
                    SourceVersion = source.SourceVersion.Truncate(2000)                    
                };

                e.AddProperties(source.Properties);
                e.AddTags(context, source.Tags, org);
                e.AddTriggers(source.Triggers);
            }

            return e;
        }


        /// <summary>
        /// Convert an InternalEvent to an API Event 
        /// </summary>
        internal static EventDetails ToEventDetails(InternalEvent source)
        {
            return source == null 
                ? null 
                : new EventDetails()
                {
                    Id = source.Id,
                    Organisation = Convert.ToOrganisation(source.Organisation),
                    Category = Enum.Parse<EventCategory>(source.Category, true),
                    Summary = source.Summary,
                    TimestampUtc = source.TimestampUtc,
                    LastUpdatedUtc = source.ModifiedOnUtc,
                    Properties = source.InternalEventProperties == null
                        ? null
                        : source.InternalEventProperties.Select(p => Convert.ToProperty(p.Property)).ToList(),
                    Source = source.Source,
                    SourceVersion = source.SourceVersion,
                    Tags = source.InternalEventTags == null
                        ? null
                        : source.InternalEventTags.Select(t => t.Tag.Name).ToList(),
					Triggers = source.Triggers == null
						? null
						: source.Triggers.Select(t => Convert.ToTrigger(t)).ToList()
                };
        }




        /// <summary>
        /// Convert an InternalEvent to an EventSummary
        /// </summary>
        internal static EventSummary ToEventSummary(InternalEvent source)
        {
            return new EventSummary()
            {
                Id = source.Id,
                Category = Enum.Parse<EventCategory>(source.Category,true),
                Summary = source.Summary,
                TimestampUtc = source.TimestampUtc,
                Source = source.Source
            };
        }
    }
}
