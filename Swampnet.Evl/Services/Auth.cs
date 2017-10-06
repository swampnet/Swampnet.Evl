using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services
{
    public interface IAuth
    {
        Task<Organisation> GetOrganisationAsync(Guid apiKey);
    }



    class Auth : IAuth
    {
        private readonly IManagementDataAccess _managementData;

        public Auth(IManagementDataAccess managementData)
        {
            _managementData = managementData;
        }


        public Task<Organisation> GetOrganisationAsync(Guid apiKey)
        {
            // @TODO: Implement a cache!
            return _managementData.LoadOrganisationAsync(apiKey);
        }
    }
}
