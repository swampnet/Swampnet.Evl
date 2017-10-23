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
        internal static InternalEvent ToEvent(EventDetails source, EvlContext context)
        {
            InternalEvent e = null;

            if (source != null)
            {
                e = new InternalEvent()
                {
                    Category = source.Category.ToString(),
                    Summary = source.Summary,
                    TimestampUtc = source.TimestampUtc,
                    LastUpdatedUtc = source.LastUpdatedUtc.HasValue ? source.LastUpdatedUtc.Value : source.TimestampUtc,
                    Source = source.Source.Truncate(2000),
                    SourceVersion = source.SourceVersion.Truncate(2000)                    
                };

                e.AddProperties(source.Properties);
                e.AddTags(context, source.Tags);
                e.AddTriggers(source.Triggers);
            }

            return e;
        }



        /// <summary>
        /// Convert an API IProperty to an InternalProperty
        /// </summary>
        internal static InternalProperty ToInternalProperty(IProperty source)
        {
            return new InternalProperty()
            {
                Category = source.Category.Truncate(225),
                Name = source.Name.Truncate(225),
                Value = source.Value == null ? "null" : source.Value
            };
        }


        /// <summary>
        /// Convert an InternalEvent to an API Event 
        /// </summary>
        internal static EventDetails ToEvent(InternalEvent source)
        {
            return source == null 
                ? null 
                : new EventDetails()
                {
                    Id = source.Id,
                    Category = Enum.Parse<EventCategory>(source.Category, true),
                    Summary = source.Summary,
                    TimestampUtc = source.TimestampUtc,
                    LastUpdatedUtc = source.LastUpdatedUtc,
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


		internal static Trigger ToTrigger(InternalTrigger source)
		{
			return new Trigger()
			{
				RuleName = source.RuleName,
				RuleId = source.RuleId,
				TimestampUtc = source.TimestampUtc,
				Actions = source.Actions == null
					? null
					: source.Actions.Select(a => Convert.ToAction(a)).ToList()
			};
		}


		internal static TriggerAction ToAction(InternalAction source)
		{
			return new TriggerAction()
			{
				Error = source.Error,
				Type = source.Type,
				TimestampUtc = source.TimestampUtc,
				Properties = source.InternalActionProperties == null
					? null
					: source.InternalActionProperties.Select(p => Convert.ToProperty(p.Property)).ToList()
			};
		}


		internal static InternalTrigger ToTrigger(Trigger source)
        {
            var trigger = new InternalTrigger();

            trigger.RuleName = source.RuleName;
			trigger.RuleId = source.RuleId;
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

        /// <summary>
        /// Convert an IProperty to a Property
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static Property ToProperty(IProperty source)
        {
            return new Property()
            {
                Category = source.Category,
                Name = source.Name,
                Value = source.Value
            };
        }
        #endregion
    }
}
