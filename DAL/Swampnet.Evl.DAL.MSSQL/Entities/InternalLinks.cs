using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// EF Core currently doesn't let us abstract this stuff away the way EF6 does. 
/// Until it does I'm keeping all the link tables in here, so when that glorious day comes I can delete them all in one hit!
/// </summary>
namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalEventProperties
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long InternalPropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }


    class InternalEventTags
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long InternalTagId { get; set; }
        public InternalTag Tag { get; set; }
    }
}
