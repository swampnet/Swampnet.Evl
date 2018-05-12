using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using System.Linq;
using Swampnet.Evl.DAL.MSSQL.Entities;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class ManagementDataAccess : IManagementDataAccess
    {
        private readonly IConfiguration _cfg;

        public ManagementDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }

     //   public async Task<Profile> LoadProfileAsync(Organisation org, long id)
     //   {
     //       using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
     //       {
     //           var p = await context.Profiles
     //               .Include(x => x.Organisation)
     //               .Include(x => x.InternalProfileRoles)
     //                   .ThenInclude(pg => pg.Role)
					//		.ThenInclude(r => r.InternalRolePermissions)
					//			.ThenInclude(r => r.Permission)
     //               .Include(x => x.Audit)
     //                   .ThenInclude(x => x.Audit)
					//.SingleOrDefaultAsync(x => x.Organisation.Id == org.Id && x.Id == id);

     //           return Convert.ToProfile(p);
     //       }
     //   }


     //   public async Task<Profile> LoadProfileAsync(string key)
     //   {
     //       using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
     //       {
     //           var p = await context.Profiles
     //               .Include(x => x.Organisation)
     //               .Include(x => x.InternalProfileRoles)
     //                   .ThenInclude(pg => pg.Role)
					//		.ThenInclude(r => r.InternalRolePermissions)
					//			.ThenInclude(r => r.Permission)
     //               .Include(x => x.Audit)
     //                   .ThenInclude(x => x.Audit)
     //               .SingleOrDefaultAsync(x => x.Key == key);

     //           return Convert.ToProfile(p);
     //       }
     //   }


        //public async Task<IEnumerable<Profile>> LoadProfilesAsync(Organisation org)
        //{
        //    using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
        //    {
        //        var p = await context.Profiles
        //            .Where(x => x.Organisation.Id == org.Id)
        //            .ToListAsync();

        //        return p.Select(Convert.ToProfile);
        //    }
        //}


        public async Task<Organisation> LoadOrganisationByApiKeyAsync(Guid apiKey)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations
                    //.Include(x => x.Audit)
                    //    .ThenInclude(x => x.Audit)
                    .FirstOrDefaultAsync(o => o.ApiKey == apiKey);

                return Convert.ToOrganisation(org);
            }
        }

        public async Task<Organisation> LoadOrganisationAsync(Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations
                    //.Include(x => x.Audit)
                    //    .ThenInclude(x => x.Audit)
                    .FirstOrDefaultAsync(o => o.Id == id);

                return Convert.ToOrganisation(org);
            }
        }
    }
}
