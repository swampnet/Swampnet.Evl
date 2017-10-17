using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Client;
using System.Threading.Tasks;
using System.Linq;
using Swampnet.Evl.DAL.InMemory.Entities;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class RuleDataAccess : IRuleDataAccess
    {
        private readonly IConfiguration _cfg;

        public RuleDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;

            Seed();
        }


        public async Task<IEnumerable<RuleSummary>> SearchAsync()
        {
            using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                var rules = await context.Rules.Where(r => r.IsActive).ToListAsync();
                return rules.Select(Convert.ToRuleSummary);
            }
        }


        public async Task<Rule> LoadAsync(Guid id)
        {
            using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                var rule = await context.Rules.SingleOrDefaultAsync(r => r.IsActive && r.Id == id);
                if(rule == null)
                {
                    return null;
                }
                return Convert.ToRule(rule);
            }
        }


        public async Task<IEnumerable<Rule>> LoadAsync(Organisation org)
        {
            using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                var rules = await context.Rules.Where(r => r.IsActive).ToListAsync();

                return rules.Select(r => Convert.ToRule(r));
            }
        }


        public async Task CreateAsync(Rule rule)
        {
            using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                rule.Id = Guid.NewGuid();
                context.Rules.Add(Convert.ToRule(rule));
                await context.SaveChangesAsync();
            }
        }


        public async Task UpdateAsync(Rule rule)
        {
            using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                var r = context.Rules.SingleOrDefault(x => x.Id == rule.Id);
                if(r == null)
                {
                    throw new NullReferenceException("Rule not found");
                }

                r.Name = rule.Name;
                r.IsActive = rule.IsActive;
                r.ActionData = rule.Actions.ToXmlString();
                r.ExpressionData = rule.Expression.ToXmlString();

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
			// @TODO: Now, do we really want to delete stuff or just flag it as so?
			//        A: Well, flag it as so, obv. Question is, do we use the active flag for that?
			using (var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
			{
				var r = context.Rules.SingleOrDefault(x => x.Id == id);
				if (r == null)
				{
					throw new NullReferenceException("Rule not found");
				}

				r.IsActive = false;

				await context.SaveChangesAsync();
			}
        }

        private void Seed()
        {
            using(var context = RuleContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                if (!context.Rules.Any())
                {
                    foreach (var r in _mockedRules)
                    {
                        context.Rules.Add(Convert.ToRule(r));
                    }
                    context.SaveChanges();
                }
            }
        }



        private static IEnumerable<Rule> _mockedRules = new [] {
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
    }
}
