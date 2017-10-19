using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("stats")]
    public class StatsController : Controller
    {
        private readonly IEventDataAccess _dal;
        private readonly IAuth _auth;

        public StatsController(IEventDataAccess dal, IAuth auth)
        {
            _dal = dal;
            _auth = auth;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stats = new Stats();
                var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);
                stats.ApiVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
                stats.TotalEvents = await _dal.GetTotalEventCountAsync(org);

                return Ok(stats);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }
    }
}
