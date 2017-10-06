using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Linq;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.DAL.InMemory
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
        #endregion
    }
}
