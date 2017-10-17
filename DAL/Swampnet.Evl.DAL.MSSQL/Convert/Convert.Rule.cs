using System;
using System.Linq;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    static partial class Convert
    {
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
                Actions = rule.Actions.Where(a => a.IsActive).Select(a => a.Type).ToArray()
            };
        }

        #endregion
    }
}
