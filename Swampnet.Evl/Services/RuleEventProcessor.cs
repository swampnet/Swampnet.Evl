using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using System.Diagnostics;

namespace Swampnet.Evl.Services
{
    public class RuleEventProcessor : IEventProcessor
    {
        private readonly IRuleLoader _loader;
        private readonly Dictionary<string, IActionHandler> _actionHandlers;

        public int Priority => 0;

        public RuleEventProcessor(IRuleLoader loader, IEnumerable<IActionHandler> actionHandlers)
        {
            _loader = loader;
            _actionHandlers = actionHandlers.ToDictionary(a => a.GetType().Name.Replace("ActionHandler", ""));
        }

        public void Process(Event evt)
        {
            var rules = _loader.Load(null).ToList();

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
                        if (rule.Expression.Evaluate(evt))
                        {
                            foreach (var action in rule.Actions)
                            {
                                if (_actionHandlers.ContainsKey(action.Type))
                                {
                                    _actionHandlers[action.Type].Apply(evt, action.Properties);
                                }
                                else
                                {
                                    // @TODO: We should throw an exception here shouldn't we?
                                }
                            }

                            // Stop processing a rule if it comes back true
                            rules.Remove(rule);
                            count++;
                        }
                    }
                }

                evt.Properties.Add(new Property("Internal", "Rules evaluated (elapsed ms)", sw.Elapsed.TotalMilliseconds));
            }
        }
    }
}
