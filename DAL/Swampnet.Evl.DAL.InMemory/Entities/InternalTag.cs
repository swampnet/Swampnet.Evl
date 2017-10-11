using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class InternalTag
    {
        public InternalTag()
        {
            InternalEventTags = new List<InternalEventTags>();
        }


        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<InternalEventTags> InternalEventTags { get; set; }
    }
}
