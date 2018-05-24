using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("organisation")]
    public class OrganisationController : Controller
    {
        private readonly IAuth _auth;
        private readonly IManagementDataAccess _management;

        public OrganisationController(IAuth auth, IManagementDataAccess management)
        {
            _auth = auth;
            _management = management;
        }


        /// <summary>
        /// Get organisation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var org = await _auth.GetOrganisationByApiKeyAsync(Request?.ApiKey());
                if (org == null)
                {
                    return Unauthorized();
                }

                return Ok(org);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]Organisation organisation)
        {
            try
            {
                var org = await _auth.GetOrganisationByApiKeyAsync(Request?.ApiKey());
                if (org == null)
                {
                    return Unauthorized();
                }

                organisation = await _management.UpdateOrganisationAsync(org.Id, organisation);

                return Ok(organisation);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }
    }
}
