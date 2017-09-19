//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Swampnet.Evl.Common.Entities;
//using Swampnet.Evl.Client;

//namespace Swampnet.Evl.Web.Controllers
//{
//    [Route("api")]
//    public class MockedController : Controller
//    {
//        [HttpGet("rules")]
//        public IEnumerable<RuleSummary> GetRules()
//        {
//            //Task.Delay(2000).Wait();
//            return _rules.Select(r => new RuleSummary(r.Id.Value, r.Name));
//        }


//        [HttpGet("rules/{id}")]
//        public Rule GetRule(Guid id)
//        {
//            //Task.Delay(1000).Wait();
//            return _rules.SingleOrDefault(r => r.Id == id);
//        }


//        [HttpGet("meta")]
//        public MetaData GetMetaData()
//        {
//            return MetaData.Default;
//        }


//        [HttpGet("events")]
//        public IEnumerable<EventSummary> Events([FromQuery] EventSearchCriteria criteria)
//        {
//            return _events.Select(e => new EventSummary() {
//                Id = e.Id.Value,
//                Category = e.Category,
//                Summary= e.Summary,
//                TimestampUtc = e.TimestampUtc
//            });
//        }


//        [HttpGet("events/{id}")]
//        public Event GetEventDetails(Guid id)
//        {
//            return _events.SingleOrDefault(e => e.Id == id);
//        }



//        private readonly static IEnumerable<Rule> _rules = new[]
//        {
//            new Rule()
//            {
//                Id = Guid.NewGuid(),
//                Name = "Some madeup junk",
//                Expression = new Expression(RuleOperatorType.MATCH_ALL)
//                {
//                    Children = new[]
//                    {
//                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, null, "Information"),
//                        new Expression(RuleOperatorType.EQ, RuleOperandType.Summary, null, "test"),
//                        new Expression(RuleOperatorType.MATCH_ANY)
//                        {
//                            Children = new[]
//                            {
//                                new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-one", "1"),
//                                new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-two", "2")
//                            }
//                        },
//                        new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-three", "3")
//                    }
//                },
//                Actions = new[]
//                {
//                    new ActionDefinition("email")
//                    {
//                        Properties = new[]
//                        {
//                            new Property("to", "pj@theswamp.co.uk"),
//                            new Property("cc", ""),
//                            new Property("bcc", ""),
//                        }
//                    },
//                    new ActionDefinition("change-category")
//                    {
//                        Properties = new[]
//                        {
//                            new Property("Category", "Information")
//                        }
//                    }
//                }
//            },
//            new Rule()
//            {
//                Id = Guid.NewGuid(),
//                Name = "Email test",
//                Expression = new Expression(RuleOperatorType.MATCH_ALL)
//                {
//                    Children = new[]
//                    {
//                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, null, "Information"),
//                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, null, "*.test-email*."),
//                    }
//                },
//                Actions = new[]
//                {
//                    new ActionDefinition("email")
//                    {
//                        Properties = new[]
//                        {
//                            new Property("to", "pj@theswamp.co.uk"),
//                            new Property("cc", ""),
//                            new Property("bcc", ""),
//                        }
//                    },
//                    new ActionDefinition("change-category")
//                    {
//                        Properties = new[]
//                        {
//                            new Property("Category", "Information")
//                        }
//                    }
//                }
//            }
//        };


//        private readonly static IEnumerable<Event> _events = new[]
//        {
//                new Event()
//                {
//                    Id = Guid.NewGuid(),
//                    Category = EventCategory.Information,
//                    Summary = "Mocked event 1",
//                    TimestampUtc = DateTime.UtcNow,
//                    Properties = new List<Property>()
//                    {
//                        new Property("name-one", "value-one"),
//                        new Property("name-two", "value-two"),
//                        new Property("name-three", "value-three")
//                    }
//                },
//                new Event()
//                {
//                    Id = Guid.NewGuid(),
//                    Category = EventCategory.Information,
//                    Summary = "Mocked event 2",
//                    TimestampUtc = DateTime.UtcNow,
//                    Properties = new List<Property>()
//                    {
//                        new Property("name-one", "value-one"),
//                        new Property("name-two", "value-two"),
//                        new Property("name-three", "value-three")
//                    }
//                }
//        };
//    }
//}
