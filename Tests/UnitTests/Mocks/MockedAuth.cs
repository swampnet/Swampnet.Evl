﻿using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using System.Security.Principal;

namespace UnitTests.Mocks
{
    class MockedAuth : IAuth
    {
        public Organisation Organisation { get; set; }

        public Organisation GetEvlOrganisation()
        {
            return Organisation;
        }

        public Task<Organisation> GetOrganisationAsync(Guid id)
        {
            return Task.FromResult(Organisation);
        }

        public Task<Organisation> GetOrganisationAsync(IPrincipal principle)
        {
            return Task.FromResult(Organisation);
        }

        public Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            return Task.FromResult(Organisation);
        }

        public Task<Profile> GetProfileAsync(IPrincipal principle)
        {
            throw new NotImplementedException();
        }
    }
}
