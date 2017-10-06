using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IManagementDataAccess
    {
        Task<Organisation> LoadOrganisationAsync(Guid apiKey);
    }
}
