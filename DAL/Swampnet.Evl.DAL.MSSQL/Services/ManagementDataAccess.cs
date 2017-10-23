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


        public async Task<Organisation> LoadOrganisationByApiKeyAsync(Guid apiKey)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations.FirstOrDefaultAsync(o => o.ApiKey == apiKey);

                return Convert.ToOrganisation(org);
            }
        }

        public async Task<Organisation> LoadOrganisationAsync(Guid orgId)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations.FirstOrDefaultAsync(o => o.Id == orgId);

                return Convert.ToOrganisation(org);
            }
        }
    }
}
