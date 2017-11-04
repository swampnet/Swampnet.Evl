using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IManagementDataAccess
    {
        Task<Organisation> LoadOrganisationByApiKeyAsync(Guid apiKey);
        Task<Organisation> LoadOrganisationAsync(Guid id);
        Task<Profile> LoadProfileAsync(string key);
        Task<Profile> LoadProfileAsync(Organisation org, long id);
        Task<IEnumerable<Profile>> LoadProfilesAsync(Organisation org);
    }
}
