using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;

namespace Swampnet.Evl.Actions
{
    class SlackActionHandler : IActionHandler
    {
        public void Apply(Event evt, IEnumerable<IProperty> properties)
        {
            var channel = properties.StringValue("channel");

            if (!channel.Any())
            {
                throw new ArgumentException("No 'channel' parameter");
            }

            Log.Information("@TODO: Post to Slack!");
        }
    }
}
