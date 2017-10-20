using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalActionProperties
    {
        public InternalActionProperties()
        {
        }

        public InternalActionProperties(InternalAction action, InternalProperty property)
            : this()
        {
            Action = action;
            Property = property;
        }

        public long ActionId { get; set; }
        public InternalAction Action { get; set; }


        public long InternalPropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }
}
