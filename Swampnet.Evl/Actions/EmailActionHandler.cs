using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;

namespace Swampnet.Evl.Actions
{
    class EmailActionHandler : IActionHandler
    {
        private const string _defaultFrom = "evl@theswamp.co.uk";

        public void Apply(Event evt, IEnumerable<IProperty> properties)
        {
            var to = properties.StringValues("to");
            var cc = properties.StringValues("cc");
            var bcc = properties.StringValues("bcc");
            var from = properties.StringValue("from", _defaultFrom);

            if (!to.Any())
            {
                throw new ArgumentException("No 'to' parameter");
            }

            Log.Information("@TODO: Send email!");
        }
    }
}
