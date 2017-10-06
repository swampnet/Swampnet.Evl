using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Swampnet.Evl.DAL.InMemory
{
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
                    ApiKey = source.ApiKey
                };
        }
    }
}
