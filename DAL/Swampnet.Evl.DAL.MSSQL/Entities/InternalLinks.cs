using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// EF Core currently doesn't let us abstract this stuff away the way EF6 does. 
/// Until it does I'm keeping all the link tables in here, so when that glorious day comes I can delete them all in one hit!
/// </summary>
namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    /// <summary>
    /// 
    /// </summary>
    class InternalEventProperties
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long PropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    class InternalEventTags
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long TagId { get; set; }
        public InternalTag Tag { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    class InternalActionProperties
    {
        public InternalActionProperties()
        {
        }

        public InternalActionProperties(InternalAction action, InternalProperty property)
            : this()
        {
            Action = action;
            Property = property;
        }

        public long ActionId { get; set; }
        public InternalAction Action { get; set; }


        public long PropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    class InternalOrganisationConfigurationProperties
    {
        public InternalOrganisationConfigurationProperties()
        {

        }

        public InternalOrganisationConfigurationProperties(InternalOrganisation organisation, InternalProperty property)
            : this()
        {
            Organisation = organisation;
            Property = property;
        }

        public Guid OrganisationId { get; set; }
        public InternalOrganisation Organisation { get; set; }


        public long PropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }
}
