using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    /// <summary>
    /// Profiles admin
    /// </summary>
    [Route("profiles")]
    public class ProfilesController : Controller
    {
        private readonly IAuth _auth;
        private readonly IManagementDataAccess _managementData;

        /// <summary>
        /// Construction
        /// </summary>
        public ProfilesController(IManagementDataAccess managementData, IAuth auth)
        {
            _managementData = managementData;
            _auth = auth;
        }


        /// <summary>
        /// Get all users in current users organisation
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

                var profiles = await _managementData.LoadProfilesAsync(profile.Organisation);

                return Ok(profiles.Select(p => new ProfileSummary() {
                    Id = p.Id,
                    Key = p.Key,
                    Name = p.Name
                }));

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Get all users in current users organisation
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null)
                {
                    return Unauthorized();
                }

                var p = await _managementData.LoadProfileAsync(profile.Organisation, id);

                if (p == null)
                {
                    return NotFound();
                }

                return Ok(p);

            }
            catch (Exception ex)
            {
                ex.AddData("Profile.Id", id);
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Get logged in users profile details
        /// </summary>
        /// <returns></returns>
        [HttpGet("current")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null)
                {
                    return Unauthorized();
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }
    }
}
