using System;
using System.Linq;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    /// <summary>
    /// Rules, Actions, Triggers
    /// </summary>
    static partial class Convert
    {
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
                Order = source.Order,
                Expression = source.ExpressionData.Deserialize<Expression>(),
                Actions = source.ActionData.Deserialize<ActionDefinition[]>(),
                Audit = source.Audit == null
                    ? null
                    : source.Audit.Select(a => ToAudit(a.Audit)).ToArray()
            };
        }

        /// <summary>
        /// Convert InternalAudit to an API Audit
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static Audit ToAudit(InternalAudit source)
        {
            return new Audit()
            {
                Action = source.Action,
                Profile = Convert.ToProfileSummary(source.Profile),
                TimestampUtc = source.TimestampUtc
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
                Order = source.Order,
                IsActive = source.IsActive,
                ExpressionData = source.Expression.ToXmlString(),
                ActionData = source.Actions.ToXmlString()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static RuleSummary ToRuleSummary(InternalRule source)
        {
            var rule = Convert.ToRule(source);
            return new Common.Entities.RuleSummary()
            {
                Id = source.Id,
                IsActive = rule.IsActive,
                Name = rule.Name,
                Order = rule.Order,
                Actions = rule.Actions.Where(a => a.IsActive).Select(a => a.Type).ToArray()
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

            if (source.Properties != null)
            {
                foreach (var p in source.Properties)
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
    }
}
