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
    class DebugActionHandler : IActionHandler
    {
        public string Type => "debug";
        public string Name => "Debug";
        public string Description => "@todo: Description for debug";

        public Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            Log.Information("Debug Action Handler");

            return Task.CompletedTask;
        }

        public MetaDataCapture[] GetPropertyMetaData()
        {
            return null;
        }

    }
}
