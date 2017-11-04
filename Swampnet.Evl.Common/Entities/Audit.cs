using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Audit
    {
        public DateTime TimestampUtc { get; set; }
        public AuditAction Action { get; set; }
        public ProfileSummary Profile { get; set; }
    }
}
