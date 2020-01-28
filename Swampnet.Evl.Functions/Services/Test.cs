using Microsoft.Extensions.Logging;
using Swampnet.Evl.Functions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swampnet.Evl.Functions.Services
{
    class Test : ITest
    {
        public IEnumerable<Event> Boosh()
        {
            return Enumerable.Range(0, 100).Select(x => new Event()
            {
                Id = Guid.NewGuid(),
                Summary = $"Event {x}",
                TimestampUtc = DateTime.Now.AddSeconds(-x)
            });
        }
    }
}
