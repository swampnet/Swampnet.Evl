using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    /// <summary>
    /// LInk table between an Event and its tags
    /// </summary>
    class InternalEventTags
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }


        public long InternalTagId { get; set; }
        public InternalTag Tag { get; set; }
    }
}
