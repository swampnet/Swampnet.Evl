using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Entities
{
    // api-key is linked to an application
    // rules are application specific
    public class Application
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public Guid ApiKey { get; set; }
    }


    public class ApplicationSummary
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
