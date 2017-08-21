using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Interfaces;
using Swampnet.Evl.Entities;
using Swampnet.Evl.Actions;

namespace UnitTests
{
    static class Mock
    {
        public static Event Event()
        {
            return new Event()
            {
                Source = "source",
                Category = "test",
                Summary = "test-summary",
                Properties = new List<Property>(Mock.Properties())
            };
        }


        internal static IRuleLoader RuleLoader(IEnumerable<Rule> rules)
        {
            return new MockedRuleLoader(rules);
        }


        public static IEnumerable<Property> Properties()
        {
            return new []
            {
                new Property("some-property", "test"),
                new Property("some-other-property", "some-other-value"),
                new Property("one", "1"),
                new Property("two", "2"),
                new Property("three", "3")
            };
        }

        public static IEnumerable<Rule> Rules()
        {
            return new[]
            {
                new Rule()
                {
                    Expression = new Expression(RuleOperatorType.MATCH_ALL)
                    {
                        Children = new List<Expression>()
                        {
                            new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
                            new Expression(RuleOperatorType.EQ, RuleOperandType.Summary, "test-summary")
                        }
                    },
                    Actions = new[]
                    {
                        new ActionDefinition("Debug")
                        {
                            Properties = new[]
                            {
                                new Property("name-01", "value-01")
                            }
                        }
                    }
                }
            };
        }


        public static IEnumerable<IActionHandler> ActionHandlers()
        {
            return new List<IActionHandler>()
            {
                new ChangeCategoryActionHandler(),
                new DebugActionHandler(),
                new AddPropertyActionHandler()
            };
        }

        private class MockedRuleLoader : IRuleLoader
        {
            private readonly IEnumerable<Rule> _rules;

            public MockedRuleLoader(IEnumerable<Rule> rules)
            {
                _rules = rules;
            }

            public IEnumerable<Rule> Load(Application app)
            {
                return _rules;
            }
        }
    }
}
