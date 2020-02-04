using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations.ActionProcessors
{
    class EmailAction : IActionProcessor
    {
        private readonly INotify _notify;

        public EmailAction(INotify notify)
        {
            _notify = notify;
        }

        public string Name => "email";

        public async Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition)
        {
            var msg = new EmailMessage()
            {
                Subject = definition.Properties.StringValue("subject")??evt.Summary,
                Body = $"<body>@todo: Body for evt:{evt.Reference}</body>"
            };

            foreach(var r in definition.Properties.StringValues("recipient"))
            {
                foreach(var part in r.Split(_recipientSplit, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()))
                {
                    if(part.StartsWith("cc:", StringComparison.OrdinalIgnoreCase))
                    {
                        msg.Cc.Add(new EmailMessage.Recipient(part.Substring(3)));
                    }
                    else if(part.StartsWith("bcc:", StringComparison.OrdinalIgnoreCase))
                    {
                        msg.Bcc.Add(new EmailMessage.Recipient(part.Substring(4)));
                    }
                    else
                    {
                        msg.To.Add(new EmailMessage.Recipient(part));
                    }
                }
            }

            await _notify.SendEmailAsync(msg);
        }

        private static readonly char[] _recipientSplit = new[] { ';' };
    }
}
