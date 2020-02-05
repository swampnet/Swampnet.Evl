using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Swampnet.Evl.Services.Implementations
{
    class RuleProcessor : IRuleProcessor
    {
        private readonly EventsContext _context;
        private readonly IEnumerable<IActionProcessor> _processors;
        private readonly IRuleRepository _rules;
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IConfigurationRoot _configuration;

        public RuleProcessor(
            EventsContext context,
            IRuleRepository rules,
            IConfigurationRoot config,
            IEnumerable<IActionProcessor> processors)
        {
            _context = context;
            _processors = processors;
            _rules = rules;
            _configuration = config;
        }


        public async Task ProcessEventAsync(Guid id)
        {
            var e = await _context.Events
                .Include(f => f.Category)
                .Include(f => f.Source)
                .Include(f => f.Properties)
                .Include(f => f.History)
                .Include(f => f.EventTags)
                    .ThenInclude(f => f.Tag)
                .SingleAsync(x => x.Reference == id);


            var sw = Stopwatch.StartNew();
            var expressionEvaluator = new ExpressionEvaluator();
            int count = int.MaxValue;

            var rules = new List<Rule>(await _cache.GetOrCreateAsync<Rule[]>("rules", LoadRules));

            // Keep processing the rules until either we run out of rules, or all the rules evaluate to false.
            // When a rule evaluates to true, run any associated actions and remove the rule from our list.
            while (count > 0 && rules.Any())
            {
                count = 0;

                foreach (var rule in rules.OrderBy(r => r.Priority).ToArray())
                {
                    // Rule is true.
                    if (expressionEvaluator.Evaluate(rule.Expression, e))
                    {
                        e.History.Add(new EventHistoryEntity() { 
                            Type = "triggered-rule",
                            Details = rule.Name
                        });

                        foreach (var action in rule.Actions.Where(a => a.IsActive))
                        {
                            // Find action & execute it against the event
                            var processor = _processors.SingleOrDefault(x => x.Name.EqualsNoCase(action.Type));
                            if (processor != null)
                            {
                                await processor.ApplyAsync(_context, e, action);

                                e.History.Add(new EventHistoryEntity()
                                {
                                    Type = "action",
                                    Details = action.Type + "(" + string.Join(",", action.Properties.Select(p => $"{p.Name}:'{p.Value}'")) + ")"
                                });
                            }
                        }

                        // Stop processing rule
                        rules.Remove(rule);

                        // Keep track of the number of rules that evaluated to true
                        count++;
                    }
                }
            }

            e.ModifiedOnUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


        private async Task<Rule[]> LoadRules(ICacheEntry arg)
        {
            arg.AbsoluteExpirationRelativeToNow = TimeSpan.Parse(_configuration["rule-cache-ttl"]);
            var rules = await _rules.LoadRulesAsync();

            return rules.Where(r => r.IsEnabled).ToArray();
        }
    }
}
