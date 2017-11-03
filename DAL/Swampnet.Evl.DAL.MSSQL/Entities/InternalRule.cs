using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    /// <summary>
    /// Entity used internally to persist a Rule
    /// </summary>
    internal class InternalRule
    {
        public InternalRule()
        {
            Audit = new List<InternalRuleAudit>();
        }

        /// <summary>
        /// PK
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FK -> Organisation
        /// </summary>
        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Navigate -> Organisation
        /// </summary>
        public InternalOrganisation Organisation { get; set; }

        /// <summary>
        /// Is the rule active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Rule name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Execution order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Serialised expression data
        /// </summary>
        public string ExpressionData { get; set; }

        /// <summary>
        /// Serialised action data
        /// </summary>
        public string ActionData { get; set; }

        /// <summary>
        /// Audit trail
        /// </summary>
        public ICollection<InternalRuleAudit> Audit { get; set; }


        internal void AddAudit(long profileId, AuditAction action)
        {
            Audit.Add(new InternalRuleAudit() {
                Audit = new InternalAudit(profileId, action),
                Rule = this
            });
        }
    }
}
