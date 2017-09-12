using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("events")]
	public class EventsController : Controller
	{
        private readonly IEventQueueProcessor _eventProcessor;
        private readonly IEventDataAccess _dal;

        public EventsController(IEventDataAccess dal, IEventQueueProcessor eventProcessor)
        {
            _dal = dal;
            _eventProcessor = eventProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EventSearchCriteria criteria)
        {
            var events = await _dal.SearchAsync(criteria);

            return Ok(events);
        }

        [HttpGet("{id}", Name = "EventDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            var evt = await _dal.ReadAsync(id);

            if(evt == null)
            {
                return NotFound();
            }

            return Ok(evt);
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

                var apiKey = Request.ApiKey();

                evt.Properties.AddRange(Request.CommonProperties());

                // @TODO: Auth

                var id = await _dal.CreateAsync(null, evt);

                _eventProcessor.Enqueue(id);

				return CreatedAtRoute("EventDetails", new { id = id }, evt);
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

                var apiKey = Request.ApiKey();
                var ids = new List<Guid>();

                // @TODO: Auth

                Parallel.ForEach(evts, async evt =>
                {
                    evt.Properties.AddRange(Request.CommonProperties());
                    var id = await _dal.CreateAsync(null, evt);
                    lock (ids)
                    {
                        ids.Add(id);
                    }
                });

                _eventProcessor.Enqueue(ids);

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
