using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
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

        public StatsController(IEventDataAccess dal)
        {
            _dal = dal;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stats = new Stats();

                stats.ApiVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
                stats.TotalEvents = await _dal.GetTotalEventCountAsync();

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
