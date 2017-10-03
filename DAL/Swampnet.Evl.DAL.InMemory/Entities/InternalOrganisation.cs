using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class InternalOrganisation
    {
        public InternalOrganisation()
        {
            Applications = new List<InternalApplication>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<InternalApplication> Applications { get; set; }
    }
}
