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
        /// Get profile from principal
        /// </summary>
        /// <param name="principle"></param>
        /// <returns></returns>
        Task<Profile> GetProfileAsync(IPrincipal principle);

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

        /// <summary>
        /// Get the organisation that represents the EVL service itself
        /// </summary>
        /// <returns></returns>
        Organisation GetEvlOrganisation();
    }



    class Auth : IAuth
    {
        class CachedProfile
        {
            public CachedProfile(Profile profile)
            {
                Profile = profile;
                CreatedOnUtc = DateTime.UtcNow;
            }

            public Profile Profile { get; private set; }
            public DateTime CreatedOnUtc { get; private set; }
        }


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
        private readonly ConcurrentDictionary<string, CachedProfile> _cachedProfiles = new ConcurrentDictionary<string, CachedProfile>();

        public Auth(IManagementDataAccess managementData, IConfiguration cfg)
        {
            _managementData = managementData;
            _cfg = cfg;
        }


        /// <summary>
        /// Get profile from IPrincipal
        /// </summary>
        /// <param name="principle"></param>
        /// <returns></returns>
        public async Task<Profile> GetProfileAsync(IPrincipal principle)
        {
            string key = Common.Constants.MOCKED_PROFILE_KEY;

            CachedProfile profile = null;

            if (_cachedProfiles.ContainsKey(key))
            {
                profile = _cachedProfiles[key];
            }
            else
            {
                var p = await _managementData.LoadProfileAsync(key);

                if (p != null)
                {
                    profile = new CachedProfile(p);
                    _cachedProfiles.TryAdd(key, profile);
                }
            }

            return profile?.Profile;
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
