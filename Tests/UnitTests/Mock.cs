using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Client;
using System.Threading.Tasks;

namespace UnitTests
{
    static class Mock
    {
        public static Event Event()
        {
            return new Event()
            {
                Source = "source",
                Category = EventCategory.Information,
                Summary = "test-summary",
                Properties = new List<Property>(Mock.Properties()),
                Tags = new List<string>(Mock.Tags())
            };
        }


        internal static IRuleDataAccess RuleLoader(IEnumerable<Rule> rules)
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
                new Property("three", "3"),
                new Property("one-point-two-three", 1.23),
                new Property("some-date", "2008-12-11 03:00:00")
            };
        }

        private static IEnumerable<string> Tags()
        {
            return new[] {
                "tag-01",
                "tag-02"
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


        private class MockedRuleLoader : IRuleDataAccess
        {
            private readonly IEnumerable<Rule> _rules;

            public MockedRuleLoader(IEnumerable<Rule> rules)
            {
                _rules = rules;
            }

            public Task CreateAsync(Rule rule)
            {
                throw new NotImplementedException();
            }

            public Task DeleteAsync(Guid id)
            {
                throw new NotImplementedException();
            }

            public Task<Rule> LoadAsync(Guid id)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Rule>> LoadAsync(Organisation org)
            {
                return Task.Run(() => _rules);
            }

            public Task<IEnumerable<RuleSummary>> SearchAsync()
            {
                throw new NotImplementedException();
            }

            public Task UpdateAsync(Rule rule)
            {
                throw new NotImplementedException();
            }
        }
    }
}
