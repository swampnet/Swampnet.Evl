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
    }
}
