﻿using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using System.Diagnostics;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Services;
using Swampnet.Evl.Client;
using Serilog;

namespace Swampnet.Evl.EventProcessors
{
    class RuleEventProcessor : IEventProcessor
    {
        private readonly IRuleDataAccess _loader;
        private readonly Dictionary<string, IActionHandler> _actionHandlers;

        public int Priority => 0;

        public RuleEventProcessor(IRuleDataAccess loader, IEnumerable<IActionHandler> actionHandlers)
        {
            _loader = loader;
            _actionHandlers = actionHandlers.ToDictionary(a => a.GetType().Name.Replace("ActionHandler", "").ToLower());
        }

        public void Process(Event evt)
        {
            var rules = _loader.Load(null).ToList();
            var expressionEvaluator = new ExpressionEvaluator();

            if (rules.Any())
            {
                var sw = Stopwatch.StartNew();
                evt.Properties.Add(new Property("Internal", "Rules evaluated", rules.Count));

                int count = int.MaxValue;

                // Keep processing the rules until either we run out of rules, or all the rules evaluate to false.
                while (count > 0 && rules.Any())
                {
                    count = 0;
                    
                    foreach (var rule in rules.ToArray())
                    {
                        if (expressionEvaluator.Evaluate(rule.Expression, evt))
                        {
                            evt.Properties.Add(new Property("Internal", "Rule Triggered", rule.Name));
                            foreach (var action in rule.Actions)
                            {
                                var key = action.Type.ToLower();

                                try
                                {
                                    if (!_actionHandlers.ContainsKey(key))
                                    {
                                        throw new InvalidOperationException($"Unknown action: {action.Type}");
                                    }

                                    _actionHandlers[key].Apply(evt, action, rule);

                                    evt.Properties.Add(new Property("Internal", "ActionApplied", action.Type));
                                }
                                catch (Exception ex)
                                {
                                    ex.AddData("Action", action.Type);
                                    Log.Error(ex, ex.Message);

                                    evt.Properties.Add(new Property("Internal", "ActionFailed", action.Type + $" ({ex.Message})"));
                                }
                            }

                            // Stop processing a rule if it comes back true
                            rules.Remove(rule);

                            // Keep track of the number of rules that evaluated to true
                            count++;
                        }
                    }
                }

                evt.Properties.Add(new Property("Internal", "Rules evaluated (elapsed ms)", sw.Elapsed.TotalMilliseconds));
            }
        }
    }
}
