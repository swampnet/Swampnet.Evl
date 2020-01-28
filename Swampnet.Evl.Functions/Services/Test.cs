using Microsoft.Extensions.Logging;
using Swampnet.Evl.Functions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Functions.Services
{
    class Test : ITest
    {
        private readonly ILogger _logger;

        public Test(ILogger logger)
        {
            _logger = logger;
        }

        public void Boosh()
        {
            _logger.LogInformation("Boosh!");
        }
    }
}
