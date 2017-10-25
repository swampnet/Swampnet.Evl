using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Organisation GetEvlOrganisation();
    }



    class Auth : IAuth
    {
        class CachedOrganisation
        {
            private TimeSpan _ttl = TimeSpan.FromMinutes(5);

            public CachedOrganisation(Organisation org)
            {
                Organisation = org;
                CreatedOn = DateTime.UtcNow;
            }


            public Organisation Organisation { get; private set; }
            public DateTime CreatedOn { get; set; }

            public bool IsExpired
            {
                get { return Age > _ttl; }
            }

            public TimeSpan Age
            {
                get { return (DateTime.UtcNow - CreatedOn); }
            }
        }

        private readonly IManagementDataAccess _managementData;
        private readonly ConcurrentDictionary<Guid, CachedOrganisation> _apiKeyCache = new ConcurrentDictionary<Guid, CachedOrganisation>();
        private readonly ConcurrentDictionary<Guid, CachedOrganisation> _idCache = new ConcurrentDictionary<Guid, CachedOrganisation>();

        public Auth(IManagementDataAccess managementData)
        {
            _managementData = managementData;
        }


        public async Task<Organisation> GetOrganisationByApiKeyAsync(Guid apiKey)
        {
            CachedOrganisation org = null;

            if (_apiKeyCache.ContainsKey(apiKey))
            {
                org = _apiKeyCache[apiKey];
            }
            else
            {
                var o = await _managementData.LoadOrganisationByApiKeyAsync(apiKey);

                if (o != null)
                {
					org = new CachedOrganisation(o);

					_apiKeyCache.TryAdd(apiKey, org);
                }
            }

            return org?.Organisation;
        }


        public async Task<Organisation> GetOrganisationAsync(Guid id)
        {
            CachedOrganisation org = null;

            if (_apiKeyCache.ContainsKey(id))
            {
                org = _apiKeyCache[id];
            }
            else
            {
                var o = await _managementData.LoadOrganisationAsync(id);

                if (o != null)
                {
                    _apiKeyCache.TryAdd(id, new CachedOrganisation(o));
                }
            }

            return org?.Organisation;
        }


        public Organisation GetEvlOrganisation()
        {
            return GetOrganisationAsync(Common.Constants.MOCKED_DEFAULT_ORGANISATION).Result;
        }
    }
}
