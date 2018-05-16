using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Swampnet.Evl.Common.Entities
{
    public class EventDetails : Event
    {
        public EventDetails()
            : base()
        {
            Triggers = new List<Trigger>();
        }


        public Guid Id { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }
        public List<Trigger> Triggers { get; set; }
    }


    public class Trigger
    {
        public Trigger()
        {
            TimestampUtc = DateTime.UtcNow;
            Actions = new List<TriggerAction>();
        }

        public Trigger(Guid ruleId, string ruleName)
            : this()
        {
            RuleName = ruleName;
			RuleId = ruleId;
        }

        public DateTime TimestampUtc { get; set; }
        public string RuleName { get; set; }
		public Guid RuleId { get; set; }
		public List<TriggerAction> Actions { get; set; }
    }


    public class TriggerAction
    {
        public TriggerAction()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public TriggerAction(ActionDefinition action)
            : this()
        {
            Type = action.Type;
            Properties = new List<Property>(action.Properties?.Where(p => !string.IsNullOrEmpty(p.Value)));
        }

        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }
        public List<Property> Properties { get; set; }
        public string Error { get; set; }
    }
}
