using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using Swampnet.Evl.Common.Entities;
using System.Collections.Generic;
using Swampnet.Evl.Client;


namespace Swampnet.Evl.DAL.MSSQL
{
    static class Seed
    {
        private static bool _init = false;

        public static void Init(string connectionString)
        {
            if (!_init)
            {
                var context = new EvlContext(new DbContextOptionsBuilder<EvlContext>()
                    .UseSqlServer(connectionString)
                    .Options);

                var databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                databaseCreator.CreateTables();

                // Organisation
                var org = new InternalOrganisation()
                {
                    Id = _mockedOrganisation.Id,
                    Name = _mockedOrganisation.Name,
                    Description = _mockedOrganisation.Description,
                    ApiKeys = new List<ApiKey>(_mockedApiKeys),
                    ApiKey = _mockedOrganisation.ApiKey
                };

                context.Organisations.Add(org);

                foreach (var r in _mockedRules)
                {
                    var rule = Convert.ToRule(r);
                    rule.OrganisationId = Common.Constants.MOCKED_DEFAULT_ORGANISATION;

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
            }
        };

        private static IEnumerable<Rule> _mockedRules = new[] {
            new Rule("Test Email")
            {
                Id = Guid.NewGuid(),
                IsActive = true,

                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-EMAIL.*")
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
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new[]
                    {
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*TEST-SLACK.*")
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

            new Rule("Test error downgrade")
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Expression = new Expression(RuleOperatorType.MATCH_ALL)
                {
                    Children = new []
                    {
                        new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "Error"),
                        new Expression(RuleOperatorType.REGEX, RuleOperandType.Summary, @".*not-an-error.*")
                    }
                },
                Actions = new[]
                {
                    new ActionDefinition("change-category", new[]
                    {
                        new Property("Category", "Information")
                    })
                }
            }
        };


        private static Organisation _mockedOrganisation = new Organisation()
        {
            Id = Common.Constants.MOCKED_DEFAULT_ORGANISATION,
            Description = "Event Logging",
            Name = "Evl",
            ApiKey = Common.Constants.MOCKED_DEFAULT_APIKEY
        };
    }
}
