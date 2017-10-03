using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.DAL.InMemory
{
    static class Convert
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

        #region Rule

        /// <summary>
        /// Convert InternalRule to an API Rule
        /// </summary>
        internal static Rule ToRule(InternalRule source)
        {
            return new Rule()
            {
                Id = source.Id,
                IsActive = source.IsActive,
                Name = source.Name,
                Expression = source.ExpressionData.Deserialize<Expression>(),
                Actions = source.ActionData.Deserialize<ActionDefinition[]>()
            };
        }


        /// <summary>
        /// Convert an API Rule to an InternalRule
        /// </summary>
        internal static InternalRule ToRule(Rule source)
        {
            return new InternalRule()
            {
                Id = source.Id.HasValue ? source.Id.Value : Guid.Empty,
                Name = source.Name,
                IsActive = source.IsActive,
                ExpressionData = source.Expression.ToXmlString(),
                ActionData = source.Actions.ToXmlString()
            };
        }
        #endregion
    }
}
