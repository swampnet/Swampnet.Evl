using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Client
{
    public class EventSearchCriteria
    {
        public Guid? Id { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
    }
}
