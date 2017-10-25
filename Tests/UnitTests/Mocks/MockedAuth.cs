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
        public Organisation Organisation { get; set; }


        public Task<Organisation> GetOrganisationAsync(Guid id)
        {
            return Task.FromResult(Organisation);
        }

        public Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            return Task.FromResult(Organisation);
        }
    }
}
