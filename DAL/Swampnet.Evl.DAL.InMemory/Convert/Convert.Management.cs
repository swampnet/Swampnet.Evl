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
        public static Application ToApplication(InternalApplication source)
        {
            return new Application()
            {
                ApiKey = source.ApiKey,
                Code = source.Code,
                Name = source.Name,
                CreatedUtc = source.CreatedUtc,
                LastUpdatedUtc = source.LastUpdatedUtc,
                Description = source.Description
            };
        }


        public static Organisation ToOrganisation(InternalOrganisation source)
        {
            return new Organisation()
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Applications = source.Applications.Select(Convert.ToApplication).ToArray()
            };
        }
    }
}
