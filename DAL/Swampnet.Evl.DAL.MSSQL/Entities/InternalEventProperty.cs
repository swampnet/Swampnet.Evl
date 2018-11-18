using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalEventProperty : IProperty
    {
        public long Id { get; set; }
        public Guid EventId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public InternalEvent Event { get; set; }
    }
}
