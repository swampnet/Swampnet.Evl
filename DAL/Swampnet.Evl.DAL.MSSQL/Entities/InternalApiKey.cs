using System;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalApiKey
    {
        public Guid Id { get; set; }
        public Guid OrganisationId { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? RevokedOnUtc { get; set; }

        public InternalOrganisation Organisation { get; set; }
    }
}
