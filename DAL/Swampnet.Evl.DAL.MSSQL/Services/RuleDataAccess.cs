using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Client;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class RuleDataAccess : IRuleDataAccess
    {
        private readonly IConfiguration _cfg;

        public RuleDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }


        public async Task<IEnumerable<RuleSummary>> SearchAsync(Organisation org)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var rules = await context.Rules.Where(r => r.IsActive).ToListAsync();
                return rules.Select(Convert.ToRuleSummary);
            }
        }


        public async Task<Rule> LoadAsync(Organisation org, Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var query = context.Rules.Where(r => r.Id == id);

                if(org != null)
                {
                    query = query.Where(r => r.OrganisationId == org.Id);
                }

                var rule = await query.SingleOrDefaultAsync();

                if(rule == null)
                {
                    return null;
                }
                return Convert.ToRule(rule);
            }
        }


        public async Task<IEnumerable<Rule>> LoadAsync(Organisation org)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var rules = await context.Rules
                                .Where(r => r.IsActive && r.OrganisationId == org.Id)
                                .ToListAsync();

                return rules.Select(r => Convert.ToRule(r));
            }
        }


        public async Task CreateAsync(Organisation org, Rule rule)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                rule.Id = Guid.NewGuid();
				var r = Convert.ToRule(rule);

                r.OrganisationId = org.Id;

				context.Rules.Add(r);
                await context.SaveChangesAsync();
            }
        }


        public async Task UpdateAsync(Organisation org, Rule rule)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var r = context.Rules
                    .SingleOrDefault(x => x.Id == rule.Id && x.OrganisationId == org.Id);

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

        public async Task DeleteAsync(Organisation org, Guid id)
        {
			// @TODO: Now, do we really want to delete stuff or just flag it as so?
			//        A: Well, flag it as so, obv. Question is, do we use the active flag for that?
			using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
			{
				var r = context.Rules.SingleOrDefault(x => x.Id == id && x.OrganisationId == org.Id);
				if (r == null)
				{
					throw new NullReferenceException("Rule not found");
				}

				r.IsActive = false;

                await context.SaveChangesAsync();
			}
        }


        public async Task ReorderAsync(Organisation org, IEnumerable<RuleOrder> rules)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                // Grab all relevent rules
                var ids = rules.Select(r => r.Id);
                var internalRules = await context.Rules.Where(r => r.OrganisationId == org.Id && ids.Contains(r.Id)).ToListAsync();
                
                // Update order
                foreach(var ro in rules)
                {
                    var rule = internalRules.Single(r => r.Id == ro.Id);
                    if(rule.Order != ro.Order)
                    {
                        rule.Order = ro.Order;
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
