//using Swampnet.Evl.Common;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Swampnet.Evl.DAL.MSSQL.Entities
//{
//    class InternalAudit
//    {
//        public InternalAudit()
//        {
//            TimestampUtc = DateTime.UtcNow;
//        }

//        public InternalAudit(long profileId, AuditAction action)
//            : this()
//        {
//            InternalProfileId = profileId;
//            Action = action;
//        }

//        public long Id { get; set; }
//        public DateTime TimestampUtc { get; set; }
//        public AuditAction Action { get; set; }
//        public InternalProfile Profile { get; set; }
//        public long? InternalProfileId { get; set; }
//    }
//}
