using Swampnet.Evl.Services;
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
        private readonly Organisation _org;

        public MockedAuth(Organisation org)
        {
            _org = org;
        }

        public Organisation GetEvlOrganisation()
        {
            return _org;
        }

        public Task<Organisation> GetOrganisationAsync(Guid id)
        {
            return Task.FromResult(_org);
        }

        public Task<Organisation> GetOrganisationAsync(IPrincipal principle)
        {
            return Task.FromResult(_org);
        }

        public Task<Organisation> GetOrganisationByApiKeyAsync(Guid? apiKey)
        {
            return Task.FromResult(_org);
        }
    }
}
