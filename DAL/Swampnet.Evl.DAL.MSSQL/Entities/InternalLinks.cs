using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// EF Core currently doesn't let us abstract this stuff away the way EF6 does. 
/// Until it does I'm keeping all the link tables in here, so when that glorious day comes I can delete them all in one hit!
/// </summary>
namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalEventProperties
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long InternalPropertyId { get; set; }
        public InternalProperty Property { get; set; }
    }


    class InternalEventTags
    {
        public Guid EventId { get; set; }
        public InternalEvent Event { get; set; }

        public long InternalTagId { get; set; }
        public InternalTag Tag { get; set; }
    }


    //class InternalRuleAudit
    //{
    //    public Guid RuleId { get; set; }
    //    public InternalRule Rule { get; set; }

    //    public long AuditId { get; set; }
    //    public InternalAudit Audit { get; set; }
    //}


    //class InternalProfileAudit
    //{
    //    public long ProfileId { get; set; }
    //    public InternalProfile Profile { get; set; }

    //    public long AuditId { get; set; }
    //    public InternalAudit Audit { get; set; }
    //}

    //class InternalOrganisationAudit
    //{
    //    public Guid OrganisationId { get; set; }
    //    public InternalOrganisation Organisation { get; set; }

    //    public long AuditId { get; set; }
    //    public InternalAudit Audit { get; set; }
    //}

    //class InternalProfileRole
    //{
    //    public long ProfileId { get; set; }
    //    public InternalProfile Profile { get; set; }

    //    public long RoleId { get; set; }
    //    public InternalRole Role { get; set; }
    //}

    //class InternalRolePermission
    //{
    //    public long RoleId { get; set; }
    //    public InternalRole Role { get; set; }

    //    public long PermissionId { get; set; }
    //    public InternalPermission Permission { get; set; }
    //}
}
