using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    class MockedAuth : IAuth
    {
        public Task<Organisation> GetOrganisationAsync(Guid id)
        {
            return Task.FromResult(Mock.MockedOrganisation());
        }

        public Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            return Task.FromResult(Mock.MockedOrganisation());
        }
    }
}
