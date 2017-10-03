using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class InternalApplication
    {
        public InternalApplication()
        {
            ApiKeys = new List<ApiKey>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public Guid ApiKey { get; set; }


        public InternalOrganisation Organisation { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; }
    }


    class ApiKey
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? RevokedOnUtc { get; set; }

        public InternalApplication Application { get; set; }
    }
}
