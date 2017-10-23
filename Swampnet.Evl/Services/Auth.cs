using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services
{
    /// <summary>
    /// Authentication
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// Get organisation from an api-key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey);

        /// <summary>
        /// Get organisation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Organisation> GetOrganisationAsync(Guid id);
    }



    class Auth : IAuth
    {
        private readonly IManagementDataAccess _managementData;
        private readonly ConcurrentDictionary<Guid, Organisation> _apiKeyCache = new ConcurrentDictionary<Guid, Organisation>();
        private readonly ConcurrentDictionary<Guid, Organisation> _idCache = new ConcurrentDictionary<Guid, Organisation>();

        public Auth(IManagementDataAccess managementData)
        {
            _managementData = managementData;
        }


        public async Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            Organisation org = null;

            if (_apiKeyCache.ContainsKey(apiKey))
            {
                org = _apiKeyCache[apiKey];
            }
            else
            {
                org = await _managementData.LoadOrganisationByApiKeyAsync(apiKey);

                if (org != null)
                {
                    _apiKeyCache.TryAdd(apiKey, org);
                }
            }

            return org;
        }


        public async Task<Organisation> GetOrganisationAsync(Guid id)
        {
            Organisation org = null;

            if (_idCache.ContainsKey(id))
            {
                org = _idCache[id];
            }
            else
            {
                org = await _managementData.LoadOrganisationAsync(id);

                if (org != null)
                {
                    _idCache.TryAdd(id, org);
                }
            }

            return org;
        }
    }
}
