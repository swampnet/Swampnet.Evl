using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalAction
    {
        public long Id { get; set; }

        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }

        public List<InternalActionProperties> InternalActionProperties { get; set; }

        #region FK Stuff
        public long TriggerId { get; set; }
        public InternalTrigger Trigger { get; set; }
        #endregion
    }
}
