using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common;
using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
	[Route("api/events")]
	public class EventsController : Controller
	{
        private readonly IEventProcessor _eventProcessor;

        public EventsController(IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;
        }


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

                evt.Properties.Add(Request.HttpContext);

                // @TODO: Auth
                // @TODO: Save evt

                _eventProcessor.Enqueue(evt);

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

                Parallel.ForEach(evts, evt =>
                {
                    evt.Properties.Add(Request.HttpContext);
                });

                // @TODO: Auth
                // @TODO: Save evts

                _eventProcessor.Enqueue(evts);

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
