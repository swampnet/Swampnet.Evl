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
    }


    class InternalProfile
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string KnownAs { get; set; }

        public Guid InternalOrganisationId { get; set; }
        public InternalOrganisation Organisation { get; set; }

        public ICollection<InternalProfileGroup> InternalProfileGroups { get; set; }
    }

    class InternalProfileGroup
    {
        public long ProfileId { get; set; }
        public InternalProfile Profile { get; set; }

        public long GroupId { get; set; }
        public InternalGroup Group { get; set; }
    }

    class InternalGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // @TODO: Permissions
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
