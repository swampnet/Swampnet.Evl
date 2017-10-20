using System;
using System.Linq;
using Swampnet.Evl.Client;
using System.Collections.Generic;
using Swampnet.Evl.DAL.MSSQL.Entities;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    static partial class Convert
    {
        #region Event

        /// <summary>
        /// Convert an API Event to an InternalEvent
        /// </summary>
        internal static InternalEvent ToEvent(EventDetails evt, EvlContext context)
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
                    Source = evt.Source.Truncate(2000),
                    SourceVersion = evt.SourceVersion.Truncate(2000)                    
                };

                e.AddProperties(evt.Properties);
                e.AddTags(context, evt.Tags);
                e.AddTriggers(evt.Triggers);
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
                Category = property.Category.Truncate(225),
                Name = property.Name.Truncate(225),
                Value = property.Value == null ? "null" : property.Value
            };
        }


        /// <summary>
        /// Convert an InternalEvent to an API Event 
        /// </summary>
        internal static EventDetails ToEvent(InternalEvent evt)
        {
            return evt == null 
                ? null 
                : new EventDetails()
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
            // @TODO: Triggers
        }


        internal static InternalTrigger ToTrigger(Trigger source)
        {
            var trigger = new InternalTrigger();

            trigger.RuleName = source.RuleName;
            trigger.TimestampUtc = source.TimestampUtc;
            trigger.Actions = source.Actions?.Select(a => Convert.ToAction(a)).ToList();

            return trigger;
        }


        internal static InternalAction ToAction(TriggerAction source)
        {
            var action = new InternalAction();

            action.TimestampUtc = source.TimestampUtc;
            action.Type = source.Type;
            action.Error = source.Error;

            if(source.Properties != null)
            {
                foreach(var p in source.Properties)
                {
                    action.InternalActionProperties.Add(new InternalActionProperties()
                    {
                        Action = action,
                        Property = Convert.ToInternalProperty(p)
                    });
                }
            }

            return action;
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
    }
}
