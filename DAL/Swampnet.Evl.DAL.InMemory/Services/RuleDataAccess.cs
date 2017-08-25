using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.DAL.InMemory.Services
{
    class RuleDataAccess : IRuleDataAccess
    {
        public IEnumerable<Rule> Load(Application app)
        {
            return new[] {
                new Rule("Test Email")
                {
                    Expression = new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-EMAIL.*"),
                    Actions = new[]
                    {
                        new ActionDefinition("email", new[]
                        {
                            new Property("to", "pj@theswamp.co.uk"),
                            new Property("cc", "pete.whitby@gmail.com"),
                        })
                    }
                },

                new Rule("Test Slack")
                {
                    Expression = new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-SLACK.*"),
                    Actions = new[]
                    {
                        new ActionDefinition("slack", new[]
                        {
                            new Property("channel", "#general")
                        })
                    }
                },

                new Rule("Test error downgrade")
                {
                    Expression = new Expression(RuleOperatorType.MATCH_ALL)
                    {
                        Children = new List<Expression>()
                        {
                            new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "error"),
                            new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*NOT-AN-ERROR.*")
                        }
                    },
                    Actions = new[]
                    {
                        new ActionDefinition("changecategory", new[]
                        {
                            new Property("category", "information")
                        })
                    }
                }
            };
        }
    }
}
