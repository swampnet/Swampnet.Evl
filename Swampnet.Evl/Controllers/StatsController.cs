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
    /// <summary>
    /// Generic stats / debug info
    /// </summary>
    [Route("stats")]
    public class StatsController : Controller
    {
        private readonly IEventDataAccess _dal;
        private readonly IAuth _auth;

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="auth"></param>
        public StatsController(IEventDataAccess dal, IAuth auth)
        {
            _dal = dal;
            _auth = auth;
        }


        /// <summary>
        /// Get all current stats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null)
                {
                    return Unauthorized();
                }

                return Ok(new Stats()
                {
                    ApiVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                    TotalEvents = await _dal.GetTotalEventCountAsync(profile.Organisation)
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }
    }
}
