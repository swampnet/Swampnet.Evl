using Microsoft.Extensions.Configuration;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Plugins.Slack
{
    class SlackActionHandler : IActionHandler
    {
        private readonly IConfiguration _cfg;
        private readonly ISlackApi _api;

        public SlackActionHandler(IConfiguration cfg, ISlackApi api)
        {
            _cfg = cfg;
            _api = api;
        }

        public string Type => "slack";


        public async Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            var msg = CreateSlackMessage(evt, actionDefinition, rule);

            await _api.PostAsync(msg);
        }

        public MetaDataCapture[] GetPropertyMetaData()
        {
            return new[]
            {
                new MetaDataCapture()
                {
                    Name = "channel",
                    Description = "Slack #channel or @usr",
                    IsRequired = true
                }
            };
        }


        // @TODO: Should probably be a service with templating and whatnopt.
        private SlackMessage CreateSlackMessage(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
			return new SlackMessage()
			{
				Token = _cfg["slack:token"],
				Channel = actionDefinition.Properties.StringValue("channel", _cfg["slack:default:channel"]),
				UserName = "evl"
			};
        }
    }
}
