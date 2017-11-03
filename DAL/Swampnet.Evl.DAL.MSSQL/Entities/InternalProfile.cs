using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalProfile
    {
        public InternalProfile()
        {
            InternalProfileRoles = new List<InternalProfileRole>();
            Audit = new List<InternalProfileAudit>();
        }


        public long Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string KnownAs { get; set; }

        public Guid InternalOrganisationId { get; set; }
        public InternalOrganisation Organisation { get; set; }
        public ICollection<InternalProfileRole> InternalProfileRoles { get; set; }
        public ICollection<InternalProfileAudit> Audit { get; set; }

        internal void AddAudit(long profileId, AuditAction action)
        {
            Audit.Add(new InternalProfileAudit()
            {
                Audit = new InternalAudit(profileId, action),
                Profile = this
            });
        }

    }
}
