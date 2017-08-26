using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Actions
{
    class SlackActionHandler : IActionHandler
    {
        public void Apply(Event evt, ActionDefinition actionDefinition, Rule rule)
        {
            var channel = actionDefinition.Properties.StringValue("channel");

            if (!channel.Any())
            {
                throw new ArgumentException("No 'channel' parameter");
            }

            Log.Information("@TODO: Post to Slack!");
        }
    }
}
