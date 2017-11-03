using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalRuleAudit
    {
        public Guid RuleId { get; set; }
        public InternalRule Rule { get; set; }


        public long AuditId { get; set; }
        public InternalAudit Audit { get; set; }
    }
}
