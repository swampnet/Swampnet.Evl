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
    /// <summary>
    /// 
    /// </summary>
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IAuth _auth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auth"></param>
        public AdminController(IAuth auth)
        {
            _auth = auth;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("organisation")]
        public async Task<IActionResult> Organisation()
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
    }
}
