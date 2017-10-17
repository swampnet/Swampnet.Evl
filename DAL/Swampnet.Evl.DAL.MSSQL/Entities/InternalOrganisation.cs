using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalOrganisation
    {
        public InternalOrganisation()
        {
            //Applications = new List<InternalApplication>();
            ApiKeys = new List<ApiKey>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApiKey { get; set; }
        //public ICollection<InternalApplication> Applications { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; }
    }


    class ApiKey
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? RevokedOnUtc { get; set; }

        public InternalOrganisation Organisation { get; set; }
    }

}
