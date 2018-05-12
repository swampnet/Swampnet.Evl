using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Swampnet.Evl.Services;
using Swampnet.Evl.Client;
using Serilog;
using System.Threading.Tasks;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.EventProcessors
{
    class RuleEventProcessor : IEventProcessor
    {
        private readonly IRuleDataAccess _rules;
        private readonly Dictionary<string, IActionHandler> _actionHandlers;

        public int Priority => 0;

        public RuleEventProcessor(IRuleDataAccess rules, IEnumerable<IActionHandler> actionHandlers)
        {
            _rules = rules;
            _actionHandlers = actionHandlers.ToDictionary(a => a.GetType().Name.Replace("ActionHandler", "").ToLower());
        }

        public async Task ProcessAsync(EventDetails evt)
        {
			var rules = new List<Rule>(await _rules.LoadAsync(evt.Organisation));

            var expressionEvaluator = new ExpressionEvaluator();

            if (rules.Any())
            {
                var sw = Stopwatch.StartNew();

                int count = int.MaxValue;

                // Keep processing the rules until either we run out of rules, or all the rules evaluate to false.
                // When a rule evaluates to true, run any associated actions and remove the rule from our list.
                while (count > 0 && rules.Any())
                {
                    count = 0;
                    
                    foreach (var rule in rules.OrderBy(r => r.Order).ToArray())
                    {
                        // Rule is true. Record a Trigger and run and actions associated with the rule.
                        if (expressionEvaluator.Evaluate(rule.Expression, evt))
                        {
                            var trigger = new Trigger(rule.Id.Value, rule.Name);

                            foreach (var action in rule.Actions)
                            {
                                var key = action.Type.Replace("-", "").ToLower();

                                var a = new TriggerAction(action);

                                try
                                {
                                    if (!_actionHandlers.ContainsKey(key))
                                    {
                                        throw new InvalidOperationException($"Unknown action: {action.Type}");
                                    }

                                    await _actionHandlers[key].ApplyAsync(evt, action, rule);
                                }
                                catch (Exception ex)
                                {
                                    ex.AddData("Action", action.Type);
                                    Log.Error(ex, ex.Message);

                                    a.Error = ex.Message;
                                }
                                finally
                                {
                                    a.TimestampUtc = DateTime.UtcNow;
                                }

                                trigger.Actions.Add(a);
                            }

                            // Add trigger to event
                            evt.Triggers.Add(trigger);

                            // Stop processing a rule if it comes back true
                            rules.Remove(rule);

                            // Keep track of the number of rules that evaluated to true
                            count++;
                        }
                    }
                }
            }
        }
    }
}
