using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
	[Route("api/events")]
	public class EventsController : Controller
	{
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Event evt)
		{
			try
			{
				if (evt == null)
				{
					return BadRequest();
				}

				await Task.Delay(1); // Just to satisfy our async declaration form now.

				var apiKey = Request.Headers[Constants.API_KEY_HEADER].SingleOrDefault();
				var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

				return Ok();
				//return CreatedAtRoute("GetEventDetails", new { id = 0 }, evt);
			}
			catch (UnauthorizedAccessException ex)
			{
				Log.Error(ex, ex.Message);
				return Unauthorized();
			}
			catch (Exception ex)
			{
				Log.Error(ex, ex.Message);
				throw;
			}
		}


        [HttpPost("bulk")]
        public async Task<IActionResult> PostBulk([FromBody] IEnumerable<Event> evts)
        {
            try
            {
                if (evts == null)
                {
                    return BadRequest();
                }

                await Task.Delay(1); // Just to satisfy our async declaration for now.

                var apiKey = Request.Headers[Constants.API_KEY_HEADER].SingleOrDefault();
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                return Ok();
                //return CreatedAtRoute("GetEventDetails", new { id = 0 }, evt);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex, ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }
    }
}
