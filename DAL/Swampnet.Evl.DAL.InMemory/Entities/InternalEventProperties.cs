using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class InternalEventProperties
    {
        public InternalEventProperties()
        {
        }

        public InternalEventProperties(InternalEvent e, InternalProperty property)
            : this()
        {
            Event = e;
            Property = property;
        }

        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }


        public long InternalPropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }
}
