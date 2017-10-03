using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
            try
            {
                Log.Logger.WithPublicProperties(criteria).Information("Get");

                var events = await _dal.SearchAsync(criteria);

                return Ok(events);
            }
            catch (Exception ex)
            {
                ex.AddData(criteria.GetPublicProperties());
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }

        [HttpGet("{id}", Name = "EventDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var evt = await _dal.ReadAsync(id);

                if (evt == null)
                {
                    return NotFound();
                }

                return Ok(evt);
            }
            catch (Exception ex)
            {
                ex.AddData("id", id);
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
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
                return this.InternalServerError(ex);
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
                    try
                    {
                        evt.Properties.AddRange(Request.CommonProperties());
                        var id = await _dal.CreateAsync(null, evt);
                        lock (ids)
                        {
                            ids.Add(id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
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
                return this.InternalServerError(ex);
            }
        }
    }
}
