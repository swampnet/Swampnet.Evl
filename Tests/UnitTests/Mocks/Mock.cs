using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Client;
using System.Threading.Tasks;
using Serilog;
using Swampnet.Evl.Services;
using Swampnet.Evl.Contracts;

namespace UnitTests.Mocks
{
    static class Mock
    {
        public static EventDetails Event()
        {
            return new EventDetails()
            {
                Source = "source",
                Category = EventCategory.Information,
                Summary = "test-summary",
                Properties = new List<Property>(Mock.Properties()),
                Tags = new List<string>(Mock.Tags())
            };
        }


        internal static ILogger Logger()
        {
            return new MockedLogger();
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

        internal static MockedEventQueueProcessor EventQueueProcessor()
        {
            return new MockedEventQueueProcessor();
        }

        internal static MockedAuth Auth(Profile profile)
        {
            return new MockedAuth()
            {
                Profile = profile
            };
        }

        internal static MockedEventDataAccess EventDataAccess()
        {
            return new MockedEventDataAccess();
        }

        internal static Profile MockedProfile()
        {
            return new Profile()
            {
                Id = 1,
                Key = "@profile-key",
                Groups = new List<Group>()
                {
                    new Group(){ Name = "tester" }
                },
                Name = new Name()
                {
                    Title = "Mr",
                    Firstname = "First",
                    Lastname = "Last",
                    KnownAs = "Testy Mc Test Face"
                },
                Organisation = MockedOrganisation()
            };
        }

        internal static Organisation MockedOrganisation()
        {
            return new Organisation()
            {
                ApiKey = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "mocked-org",
                Description = "Mocked Organisation"
            };
        }
    }
}
