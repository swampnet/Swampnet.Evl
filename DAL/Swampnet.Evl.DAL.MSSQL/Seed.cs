using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using Swampnet.Evl.Common.Entities;
using System.Collections.Generic;
using Swampnet.Evl.Client;
using System.Linq;


namespace Swampnet.Evl.DAL.MSSQL
{
    static class Seed
    {
        private static bool _init = false;

        /// <summary>
        /// Create tables and populate with some default / mocked data
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Init(string connectionString)
        {
            if (!_init)
            {
                var context = new EvlContext(new DbContextOptionsBuilder<EvlContext>()
                    .UseSqlServer(connectionString)
                    .Options);

                ((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).CreateTables();

                context.Organisations.AddRange(_mockedOrganisations.Select(o => new InternalOrganisation()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    ApiKeys = new List<ApiKey>(_mockedApiKeys),
                    ApiKey = o.ApiKey
                }));

                context.Groups.AddRange(new[]
                {
                    new InternalGroup() { Name = "owner" },
                    new InternalGroup() { Name = "admin" },
                    new InternalGroup() { Name = "user" }
                });

                context.SaveChanges();

                foreach(var p in _mockedProfiles)
                {
                    var profile = new InternalProfile()
                    {
                        Title = p.Name.Firstname,
                        Firstname = p.Name.Firstname,
                        Lastname = p.Name.Lastname,
                        KnownAs = p.Name.KnownAs,
                        Key = p.Key,
                        Organisation = context.Organisations.Single(o => o.Id == Common.Constants.MOCKED_DEFAULT_ORGANISATION),
                        InternalProfileGroups = new List<InternalProfileGroup>()
                    };

                    foreach(var g in p.Groups)
                    {
                        profile.InternalProfileGroups.Add(new InternalProfileGroup() {
                            Profile = profile,
                            Group = context.Groups.Single(x => x.Name == g.Name)
                        });
                    }

                    context.Profiles.Add(profile);
                }

                context.SaveChanges();

                foreach (var r in _mockedRules)
                {
                    var rule = Convert.ToRule(r);
                    rule.OrganisationId = Common.Constants.MOCKED_DEFAULT_ORGANISATION;
                    rule.CreatedOnUtc = DateTime.UtcNow;
                    rule.ModifiedOnUtc = DateTime.UtcNow;

                    context.Rules.Add(rule);
                }

                context.SaveChanges();
                _init = true;
            }
        }


        private static readonly List<ApiKey> _mockedApiKeys = new List<ApiKey>()
        {
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Common.Constants.MOCKED_DEFAULT_APIKEY,
                RevokedOnUtc = null
            },
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Guid.Parse("58BAD582-C6CF-407A-B482-502FB423CD55"),
                RevokedOnUtc = null
            },
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Guid.Parse("25C135A0-B574-4A9B-BC37-4F0694017896"),
                RevokedOnUtc = null
            }
        };

        private static IEnumerable<Rule> _mockedRules = new[] {
            new Rule("Test Email")
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Order = 1,
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-EMAIL.*"),
                        new Expression(RuleOperatorType.MATCH_ANY)
                        {
                            Children = new[]
                            {
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Category, EventCategory.Debug),
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Category, EventCategory.Information)
                            }
                        }
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("email", new[]
                    {
                        new Property("to", "pj@theswamp.co.uk"),
                        new Property("cc", null),
                        new Property("bcc", null)
                    })
                }
            },

            new Rule("Test Slack")
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Order = 2,
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-SLACK.*"),
                        new Expression(RuleOperatorType.MATCH_ANY)
                        {
                            Children = new[]
                            {
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Category, EventCategory.Debug),
                                new Expression(RuleOperatorType.EQ, RuleOperandType.Category, EventCategory.Information)
                            }
                        }
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("slack", new[]
                    {
                        new Property("channel", "#general")
                    })
                }
            },

            //new Rule("Startup")
            //{
            //    Id = Guid.NewGuid(),
            //    IsActive = true,
            //    Order = 3,
            //    Expression = new Expression(RuleOperatorType.MATCH_ALL)
            //    {
            //        Children = new[]
            //        {
            //            new Expression(RuleOperatorType.TAGGED, "START")
            //        }
            //    },
            //    Actions = new[]
            //    {
            //        new ActionDefinition("email", new[]{ new Property("to", "pj@theswamp.co.uk") })
            //    }
            //},

            new Rule("Test error downgrade")
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Order = 0,
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new []
                    {
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, EventCategory.Error),
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*not-an-error.*")
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("change-category", new[]
                    {
                        new Property("Category", EventCategory.Warning)
                    })
                }
            }
        };


        private static Organisation[] _mockedOrganisations = new[]
        {
            new Organisation()
            {
                Id = Common.Constants.MOCKED_DEFAULT_ORGANISATION,
                Description = "Mocked organisation",
                Name = "Mocked",
                ApiKey = Common.Constants.MOCKED_DEFAULT_APIKEY
            },
            new Organisation()
            {
                Id = Guid.Parse("60FACD3A-2232-4E15-9F3C-61289CDDD544"),
                Description = "Event Logging",
                Name = "Evl",
                ApiKey = Guid.Parse("25C135A0-B574-4A9B-BC37-4F0694017896")
            }
        };


        private static Profile[] _mockedProfiles = new[]
        {
            new Profile()
            {
                Name = new Name()
                {
                    Firstname = "Pete",
                    Lastname = "Whitby",
                    Title = "Mr",
                    KnownAs = "pj"
                },
                Key = "@todo-pjw-001",
                Groups = new List<Group>()
                {
                    new Group(){ Name = "admin"},
                    new Group(){ Name = "user"}
                }
            }
        };
    }
}
