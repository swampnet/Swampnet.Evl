using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Linq;

namespace Swampnet.Evl.Plugins.Slack
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
