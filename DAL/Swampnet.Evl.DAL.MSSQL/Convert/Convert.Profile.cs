using System.Linq;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL
{
    static partial class Convert
    {
        internal static ProfileSummary ToProfileSummary(InternalProfile source)
        {
            return new ProfileSummary()
            {
                Id = source.Id,
                Key = source.Key,
                Name = new Name()
                {
                    Title = source.Title,
                    Firstname = source.Firstname,
                    Lastname = source.Lastname,
                    KnownAs = source.KnownAs
                }
            };
        }


        internal static Profile ToProfile(InternalProfile source)
        {
            return new Profile()
            {
                Id = source.Id,
                Key = source.Key,
                Name = new Name()
                {
                    Title = source.Title,
                    Firstname = source.Firstname,
                    Lastname = source.Lastname,
                    KnownAs = source.KnownAs
                },
                Organisation = source.Organisation == null
                    ? null
                    : Convert.ToOrganisation(source.Organisation),
                Roles = source.InternalProfileRoles == null
                    ? null
                    : source.InternalProfileRoles.Select(pg => Convert.ToRole(pg.Role)).ToList()
            };
        }

        internal static Role ToRole(InternalRole source)
        {
            return new Role()
            {
                Name = source.Name,
                Permissions = source.InternalRolePermissions == null
                    ? Enumerable.Empty<Permission>()
                    : source.InternalRolePermissions.Select(rp => Convert.ToPermission(rp.Permission))

            };
        }


        internal static Permission ToPermission(InternalPermission source)
        {
            return new Permission()
            {
                IsEnabled = source.IsEnabled,
                Name = source.Name
            };
        }

    }
}
