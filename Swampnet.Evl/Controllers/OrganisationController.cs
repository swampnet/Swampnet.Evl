using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    /// <summary>
    /// Organisation admin
    /// </summary>
    [Route("organisation")]
    public class OrganisationController : Controller
    {
        private readonly IAuth _auth;
        private readonly IManagementDataAccess _managementData;

        /// <summary>
        /// Construction
        /// </summary>
        public OrganisationController(IManagementDataAccess managementData, IAuth auth)
        {
            _managementData = managementData;
            _auth = auth;
        }


        /// <summary>
        /// Get logged in users organisation details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // @TODO: Auth
                var org = await _auth.GetOrganisationAsync(Common.Constants.MOCKED_DEFAULT_ORGANISATION);

                return Ok(org);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }
    }
}
