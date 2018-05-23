using Microsoft.Extensions.Configuration;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Concurrent;
using System.Security.Principal;
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
        Task<Organisation> GetOrganisationByApiKeyAsync(Guid? apiKey);

        /// <summary>
        /// Get organisation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Organisation> GetOrganisationAsync(Guid id);

        /// <summary>
        /// Get the organisation that represents the EVL service itself
        /// </summary>
        /// <returns></returns>
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
        private readonly IConfiguration _cfg;
        private readonly ConcurrentDictionary<Guid, CachedOrganisation> _apiKeyCache = new ConcurrentDictionary<Guid, CachedOrganisation>();
        private readonly ConcurrentDictionary<Guid, CachedOrganisation> _idCache = new ConcurrentDictionary<Guid, CachedOrganisation>();
        //private readonly ConcurrentDictionary<string, CachedProfile> _cachedProfiles = new ConcurrentDictionary<string, CachedProfile>();

        public Auth(IManagementDataAccess managementData, IConfiguration cfg)
        {
            _managementData = managementData;
            _cfg = cfg;
        }



        public async Task<Organisation> GetOrganisationByApiKeyAsync(Guid? apiKey)
        {
            if(!apiKey.HasValue || apiKey == Guid.Empty)
            {
                return null;
            }

            CachedOrganisation org = null;

            if (_apiKeyCache.ContainsKey(apiKey.Value))
            {
                org = _apiKeyCache[apiKey.Value];
            }
            else
            {
                var o = await _managementData.LoadOrganisationByApiKeyAsync(apiKey.Value);

                if (o != null)
                {
					org = new CachedOrganisation(o);

					_apiKeyCache.TryAdd(apiKey.Value, org);
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
                    org = new CachedOrganisation(o);
                    _apiKeyCache.TryAdd(id, org);
                }
            }

            return org?.Organisation;
        }



        public Organisation GetEvlOrganisation()
        {
            var id = _cfg["evl:org-id"];
            if (string.IsNullOrEmpty(id))
            {
                throw new ApplicationException("evl:org-id undefined");
            }

            return GetOrganisationAsync(Guid.Parse(id)).Result;
        }
    }
}
