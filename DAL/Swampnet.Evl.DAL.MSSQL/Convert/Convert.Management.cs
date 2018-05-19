using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    /// <summary>
    /// Organisations
    /// </summary>
    static partial class Convert
    {
        public static Organisation ToOrganisation(InternalOrganisation source)
        {

            return source == null 
                ? null 
                : new Organisation()
                {
                    Id = source.Id,
                    Name = source.Name,
                    Description = source.Description,
                    ApiKeys = source.ApiKeys.Select(k => k.Id).ToArray(),
                    Properties = source.GetConfigurationProperties().Select(p => ToProperty(p)).ToArray()
                };
        }
    }
}
