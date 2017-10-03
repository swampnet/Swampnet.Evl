using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("organisation")]
    public class OrganisationController : Controller
    {
        private readonly IManagementDataAccess _managementData;

        public OrganisationController(IManagementDataAccess managementData)
        {
            _managementData = managementData;
        }


        // GET organisation
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // @TODO: Auth
                var org = await _managementData.LoadOrganisationAsync();

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
