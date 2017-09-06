using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swampnet.Evl.Web.Entities;

namespace Swampnet.Evl.Web.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<RuleSummary> GetRules()
        {
            //Task.Delay(2000).Wait();
            return _rules.Select(r => new RuleSummary(r.Id, r.Name));
        }


        [HttpGet("[action]/{id}")]
        public Rule GetRule(string id)
        {
            //Task.Delay(1000).Wait();
            return _rules.SingleOrDefault(r => r.Id == id);
        }


        [HttpGet("[action]")]
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
                                Name = "category",
                                IsRequired = true,
                                DataType = "select",
                                Options = new[]
                                {
                                    new Option("Information", "inf"),
                                    new Option("Error", "err"),
                                    new Option("Debug", "dbg")
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
                    new ExpressionOperator("match_all", "Match All", true),
                    new ExpressionOperator("match_any", "Match Any", true),

                    new ExpressionOperator("eq", "=", false),
                    new ExpressionOperator("not_eq", "<>", false),
                    new ExpressionOperator("regex", "Match Expression", false)
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
                            new Option("Information", "inf"),
                            new Option("Error", "err"),
                            new Option("Debug", "dbg")
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
                Id = "id-01",
                Name = "Some madeup junk",
                Expression = new Expression("match_all")
                {
                    Children = new[]
                    {
                        new Expression("eq", "Category", null, "info"),
                        new Expression("eq", "Summary", null, "test"),
                        new Expression("match_any")
                        {
                            Children = new[]
                            {
                                new Expression("eq", "Property", "prop-one", "1"),
                                new Expression("eq", "Property", "prop-two", "2")
                            }
                        },
                        new Expression("eq", "Property", "prop-three", "3")
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
                            new Property("category", "inf")
                        }
                    }
                }
            },
            new Rule()
            {
                Id = "id-02",
                Name = "Email test",
                Expression = new Expression("match_all")
                {
                    Children = new[]
                    {
                        new Expression("eq", "Category", null, "info"),
                        new Expression("regex", "Summary", null, "*.test-email*."),
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
                    }
                }
            }
        };
    }
}
