using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.Web.Controllers
{
    [Route("api/mocked")]
    public class MockedController : Controller
    {
        [HttpGet("rules")]
        public IEnumerable<RuleSummary> GetRules()
        {
            //Task.Delay(2000).Wait();
            return _rules.Select(r => new RuleSummary(r.Id.Value, r.Name));
        }


        [HttpGet("rules/{id}")]
        public Rule GetRule(Guid id)
        {
            //Task.Delay(1000).Wait();
            return _rules.SingleOrDefault(r => r.Id == id);
        }


        [HttpGet("meta")]
        public MetaData GetMetaData()
        {
            return new MetaData
            {
                ActionMetaData = new[]
                {
                    new ActionMetaData()
                    {
                        Type = "email",
                        Properties = new[]
                        {
                            new MetaDataCapture()
                            {
                                Name = "to",
                                IsRequired = true,
                                Options = new[]
                                {
                                    new Option(null, ".*@.*")
                                }
                            },
                            new MetaDataCapture()
                            {
                                Name = "cc",
                                IsRequired = false,
                                Options = new[]
                                {
                                    new Option(null, ".*@.*")
                                }
                            },
                            new MetaDataCapture()
                            {
                                Name = "bcc",
                                IsRequired = false,
                                Options = new[]
                                {
                                    new Option(null, ".*@.*")
                                }
                            },
                        }
                    },

                    new ActionMetaData()
                    {
                        Type = "change-category",
                        Properties = new[]
                        {
                            new MetaDataCapture()
                            {
                                Name = "Category",
                                IsRequired = true,
                                DataType = "select",
                                Options = new[]
                                {
                                    new Option("Information", "Information"),
                                    new Option("Error", "Error"),
                                    new Option("Debug", "Debug")
                                }
                            }
                        }
                    },

                    new ActionMetaData()
                    {
                        Type = "add-property",
                        Properties = new[]
                        {
                            new MetaDataCapture()
                            {
                                Name = "category",
                                IsRequired = false,
                            },
                            new MetaDataCapture()
                            {
                                Name = "name",
                                IsRequired = true,
                            },
                            new MetaDataCapture()
                            {
                                Name = "value",
                                IsRequired = true,
                            }
                        }
                    }
                },

                Operators = new[]
                {
                    new ExpressionOperator(RuleOperatorType.MATCH_ALL, "Match All", true),
                    new ExpressionOperator(RuleOperatorType.MATCH_ANY, "Match Any", true),

                    new ExpressionOperator(RuleOperatorType.EQ, "=", false),
                    new ExpressionOperator(RuleOperatorType.NOT_EQ, "<>", false),
                    new ExpressionOperator(RuleOperatorType.REGEX, "Match Expression", false)
                },

                Operands = new[]
                {
                    new MetaDataCapture()
                    {
                        Name = "Summary"
                    },

                    new MetaDataCapture()
                    {
                        Name = "Timestamp",
                        DataType = "datetime"
                    },

                    new MetaDataCapture()
                    {
                        Name = "Category",
                        DataType = "select",
                        Options = new[]
                        {
                            new Option("Information", "Information"),
                            new Option("Error", "Error"),
                            new Option("Debug", "Debug")
                        }
                    },

                    new MetaDataCapture()
                    {
                        Name = "Property",
                        DataType = "require-args"
                    }
                }
            };
        }


        private readonly static IEnumerable<Rule> _rules = new[]
        {
            new Rule()
            {
                Id = Guid.NewGuid(),
                Name = "Some madeup junk",
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, null, "Information"),
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Summary, null, "test"),
                        new Expression(RuleOperatorType.MATCH_ANY)
                        {
                            Children = new[]
                            {
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-one", "1"),
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-two", "2")
                            }
                        },
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "prop-three", "3")
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("email")
                    {
                        Properties = new[]
                        {
                            new Property("to", "pj@theswamp.co.uk"),
                            new Property("cc", ""),
                            new Property("bcc", ""),
                        }
                    },
                    new ActionDefinition("change-category")
                    {
                        Properties = new[]
                        {
                            new Property("Category", "Information")
                        }
                    }
                }
            },
            new Rule()
            {
                Id = Guid.NewGuid(),
                Name = "Email test",
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, null, "Information"),
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, null, "*.test-email*."),
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("email")
                    {
                        Properties = new[]
                        {
                            new Property("to", "pj@theswamp.co.uk"),
                            new Property("cc", ""),
                            new Property("bcc", ""),
                        }
                    },
					new ActionDefinition("change-category")
					{
						Properties = new[]
						{
							new Property("Category", "Information")
						}
					}
				}
            }
        };
    }
}
