﻿using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using System.Linq;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class ManagementDataAccess : IManagementDataAccess
    {
        public ManagementDataAccess()
        {
            Seed();
        }


        public async Task<Organisation> LoadOrganisationAsync(Guid apiKey)
        {
            using (var context = ManagementContext.Create())
            {
                var org = await context.Organisations.FirstOrDefaultAsync(o => o.ApiKey == apiKey);

                return Convert.ToOrganisation(org);
            }
        }


        private void Seed()
        {
            using (var context = ManagementContext.Create())
            {
				var org = new InternalOrganisation()
				{
					Id = Guid.NewGuid(),
					Name = "ACME Ltd",
					Description = "Some mocked up organisation.\nAuto generated.",
                    ApiKeys = new List<ApiKey>(_mockedApiKeys),
                    ApiKey = _mockedApiKeys.First().Id
				};

				context.Organisations.Add(org);
                context.SaveChanges();
            }
        }


        private static readonly List<ApiKey> _mockedApiKeys = new List<ApiKey>()
        {
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Common.Constants.MOCKED_DEFAULT_APIKEY,
                RevokedOnUtc = null
            },
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Guid.Parse("58BAD582-C6CF-407A-B482-502FB423CD55"),
                RevokedOnUtc = null
            }
        };        
    }
}
