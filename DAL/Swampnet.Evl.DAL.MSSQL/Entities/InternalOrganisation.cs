using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalOrganisation
    {
        public InternalOrganisation()
        {
            ApiKeys = new List<ApiKey>();
            Events = new List<InternalEvent>();
            Rules = new List<InternalRule>();
            Tags = new List<InternalTag>();
            Profiles = new List<InternalProfile>();
            Audit = new List<InternalOrganisationAudit>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        /// Current active ApiKey
        /// </summary>
        public Guid ApiKey { get; set; }

        public ICollection<ApiKey> ApiKeys { get; set; }
        public ICollection<InternalEvent> Events { get; set; }
        public ICollection<InternalRule> Rules { get; set; }
        public ICollection<InternalTag> Tags { get; set; }
        public ICollection<InternalProfile> Profiles { get; set; }
        public ICollection<InternalOrganisationAudit> Audit { get; set; }

        internal void AddAudit(long profileId, AuditAction action)
        {
            Audit.Add(new InternalOrganisationAudit()
            {
                Audit = new InternalAudit(profileId, action),
                Organisation = this
            });
        }

    }


    class InternalProfileRole
    {
        public long ProfileId { get; set; }
        public InternalProfile Profile { get; set; }

        public long RoleId { get; set; }
        public InternalRole Role { get; set; }
    }

    class InternalRole
    {
		public InternalRole()
		{
			InternalRolePermissions = new List<InternalRolePermission>();
		}

        public long Id { get; set; }
        public string Name { get; set; }

		public ICollection<InternalRolePermission> InternalRolePermissions { get; set; }
	}


	class InternalRolePermission
	{
		public long RoleId { get; set; }
		public InternalRole Role { get; set; }

		public long PermissionId { get; set; }
		public InternalPermission Permission { get; set; }
	}


	class InternalPermission
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public bool IsEnabled { get; set; }
	}


    class ApiKey
    {
        public Guid Id { get; set; }
        public Guid OrganisationId { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? RevokedOnUtc { get; set; }

        public InternalOrganisation Organisation { get; set; }
    }

}
