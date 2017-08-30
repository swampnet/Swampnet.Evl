using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    internal class InternalRule
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }

        // Serialised rule data
        public string ExpressionData { get; set; }
        public string ActionData { get; set; }
    }
}
