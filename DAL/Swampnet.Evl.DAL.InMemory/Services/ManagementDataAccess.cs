using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using Swampnet.Evl.DAL.InMemory.Entities;
using System.Linq;

namespace Swampnet.Evl.DAL.InMemory.Services
{
    class ManagementDataAccess : IManagementDataAccess
    {
        public ManagementDataAccess()
        {
            Seed();
        }

        // @TODO: Should be current users organisation
        public async Task<Organisation> LoadOrganisationAsync()
        {
            using (var context = ManagementContext.Create())
            {
                var org = await context.Organisations
                    .Include(o => o.Applications)
                    .FirstOrDefaultAsync();

                return Convert.ToOrganisation(org);
            }
        }


        // We might not need this, depends how the front end turns out. We get all the application
        // stuff in the Organisation object
        public async Task<Application> LoadApplicationAsync(Guid organisationId, string applicationCode)
        {
            using (var context = ManagementContext.Create())
            {
                var app = await context.Applications.SingleOrDefaultAsync(a => a.Organisation.Id == organisationId && a.Code == applicationCode);

                return Convert.ToApplication(app);
            }
        }


        private void Seed()
        {
            using (var context = ManagementContext.Create())
            {
                context.Organisations.Add(_mockedOrganisation);
                context.SaveChanges();
            }
        }


        private static readonly ApiKey[] _mockedApiKeys = new[]
        {
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Guid.Parse("3B94A54F-FDF2-4AFF-AA80-A35ED5836841"),
                RevokedOnUtc = null
            },
            new ApiKey()
            {
                CreatedOnUtc = DateTime.UtcNow,
                Id = Guid.Parse("58BAD582-C6CF-407A-B482-502FB423CD55"),
                RevokedOnUtc = null
            }
        };


        private static readonly InternalApplication[] _mockedApplications = new[]
        {
            new InternalApplication()
            {
                ApiKeys = _mockedApiKeys,
                ApiKey = _mockedApiKeys.First().Id,
                Code = "default-app",
                Name = "Default",
                CreatedUtc = DateTime.UtcNow,
                LastUpdatedUtc = DateTime.UtcNow,
                Description = "Mocked application"                
            }/*,
            new InternalApplication()
            {
                ApiKeys = _mockedApiKeys,
                ApiKey = _mockedApiKeys.Last().Id,
                Code = "second-app",
                Name = "Some other app",
                CreatedUtc = DateTime.UtcNow,
                LastUpdatedUtc = DateTime.UtcNow,
                Description = "Another mocked application"
            }*/
        };

        private static readonly InternalOrganisation _mockedOrganisation = new InternalOrganisation()
        {            
            Id = Guid.NewGuid(),
            Name = "ACME Ltd",
            Description = "Some mocked up organisation.\nAuto generated.",
            Applications = _mockedApplications.ToList()
        };
    }
}
