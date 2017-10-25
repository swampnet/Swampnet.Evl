﻿using System;
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