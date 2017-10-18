using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services
{
    public interface IAuth
    {
        Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey);
    }



    class Auth : IAuth
    {
        private readonly IManagementDataAccess _managementData;
        private readonly ConcurrentDictionary<Guid, Organisation> _cache = new ConcurrentDictionary<Guid, Organisation>();

        public Auth(IManagementDataAccess managementData)
        {
            _managementData = managementData;
        }


        public async Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            Organisation org = null;

            if (_cache.ContainsKey(apiKey))
            {
                org = _cache[apiKey];
            }
            else
            {
                org = await _managementData.LoadOrganisationByApiKeyAsync(apiKey);

                if (org != null)
                {
                    _cache.TryAdd(apiKey, org);
                }
            }

            return org;
        }
    }
}
