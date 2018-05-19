using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalOrganisation
    {
        public InternalOrganisation()
        {
            ApiKeys = new List<InternalApiKey>();
            Events = new List<InternalEvent>();
            Rules = new List<InternalRule>();
            Tags = new List<InternalTag>();
            InternalOrganisationProperties = new List<InternalOrganisationProperties>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<InternalApiKey> ApiKeys { get; set; }
        public ICollection<InternalEvent> Events { get; set; }
        public ICollection<InternalRule> Rules { get; set; }
        public ICollection<InternalTag> Tags { get; set; }
        public List<InternalOrganisationProperties> InternalOrganisationProperties { get; set; }


        internal IEnumerable<InternalProperty> GetConfigurationProperties()
        {
            return InternalOrganisationProperties == null
                ? Enumerable.Empty<InternalProperty>()
                : InternalOrganisationProperties.Select(p => p.Property);
        }
    }
}
