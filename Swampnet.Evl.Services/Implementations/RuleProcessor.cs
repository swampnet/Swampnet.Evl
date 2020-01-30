﻿using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL.Entities;
using System.Diagnostics;

namespace Swampnet.Evl.Services.Implementations
{
    class RuleProcessor : IRuleProcessor
    {
        private readonly EventsContext _context;
        private readonly IEnumerable<IActionProcessor> _processors;

        private readonly RuleEntity[] _rules = new[] { 
            new RuleEntity()
            {
                Name = "rule-01",
                Order = 1,
                Expression = new Expression(ExpressionOperatorType.MATCH_ALL)
                {
                    Children = new Expression[]
                    { 
                        new Expression(ExpressionOperatorType.EQ, ExpressionOperandType.Summary, "test-rule-01"),
                        new Expression(ExpressionOperatorType.EQ, ExpressionOperandType.Category, "info")
                    }
                },
                Actions = new[]
                { 
                    new ActionDefinition(                        
                        "add-tag", 
                        new []
                        { 
                            new Property("tag", "rule-01")
                        })
                }
            }
        };

        public RuleProcessor(EventsContext context, IEnumerable<IActionProcessor> processors)
        {
            _context = context;
            _processors = processors;
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


            if (_rules.Any())
            {
                var sw = Stopwatch.StartNew();
                var expressionEvaluator = new ExpressionEvaluator();
                var rules = new List<RuleEntity>(_rules);
                int count = int.MaxValue;

                // Keep processing the rules until either we run out of rules, or all the rules evaluate to false.
                // When a rule evaluates to true, run any associated actions and remove the rule from our list.
                while (count > 0 && rules.Any())
                {
                    count = 0;

                    foreach (var rule in rules.OrderBy(r => r.Order).ToArray())
                    {
                        // Rule is true.
                        if (expressionEvaluator.Evaluate(rule.Expression, e))
                        {
                            foreach (var action in rule.Actions.Where(a => a.IsActive))
                            {
                                var key = action.Type.Replace("-", "").ToLower();

                                var processor = _processors.SingleOrDefault(x => x.Name.EqualsNoCase(action.Type));
                                if(processor != null)
                                {
                                    await processor.ApplyAsync(_context, e, action);

                                    e.History.Add(new EventHistoryEntity()
                                    {
                                        Type = "action:" + action.Type,
                                        Details = string.Join(",", action.Properties.Select(p => $"{p.Name}={p.Value}"))
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
            }

            e.History.Add(new EventHistoryEntity() { 
                Type = "rules-complete"
            });

            e.ModifiedOnUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
