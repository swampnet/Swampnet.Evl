using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Organisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public Guid ApiKey { get; set; }
        //public ApplicationSummary[] Applications { get; set; }
    }
}
