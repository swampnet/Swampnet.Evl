using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IManagementDataAccess
    {
        /// <summary>
        /// Load currently authenticated users organisation
        /// </summary>
        /// <returns></returns>
        Task<Organisation> LoadOrganisationAsync();


        /// <summary>
        /// Load application by name.
        /// </summary>
        /// <remarks>
        /// Name should be unique within an organisation
        /// </remarks>
        Task<Application> LoadApplicationAsync(Guid organisationId, string applicationCode);
    }
}
