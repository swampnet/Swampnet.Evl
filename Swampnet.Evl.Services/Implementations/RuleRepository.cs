using Swampnet.Evl.Services.DAL.Entities;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class RuleRepository : IRuleRepository
    {
        private readonly RuleEntity[] _rules = new[] {
            new RuleEntity()
            {
                Name = "rule-01",
                Priority = 1,
                Expression = new Expression(ExpressionOperatorType.MATCH_ALL)
                {
                    Children = new Expression[]
                    {
                        new Expression(ExpressionOperatorType.EQ, ExpressionOperandType.Summary, "test-rule-01"),
                        new Expression(ExpressionOperatorType.EQ, ExpressionOperandType.Category, "info")
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition(
                        "add-tag",
                        new []
                        {
                            new Property("tag", "rule-01")
                        }),
                    new ActionDefinition(
                        "remove-tag",
                        new []
                        {
                            new Property("tag", "tag-01")
                        }),
                    new ActionDefinition(
                        "add-property",
                        new []
                        {
                            new Property("a", "a-value"),
                            new Property("b", "b-value"),
                            new Property("c", "c-value")
                        }),
                    new ActionDefinition(
                        "set-category",
                        new []
                        {
                            new Property("category", "debug")
                        })
                }
            }
        };


        public Task<Rule[]> LoadRulesAsync()
        {
            return Task.FromResult(_rules.Select(r => new Rule() {
                Id = r.Id,
                Name = r.Name,
                Priority = r.Priority,
                // @todo: Possibly deserialize at this point?
                Expression = r.Expression,
                Actions = r.Actions
            }).ToArray());
        }
    }
}
