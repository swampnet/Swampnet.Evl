﻿using Microsoft.AspNetCore.Mvc;
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
    /// All things Event based
    /// </summary>
    [Route("events")]
	public class EventsController : Controller
	{
        private readonly IEventQueueProcessor _eventProcessor;
        private readonly IEventDataAccess _dal;
        private readonly IAuth _auth;

        /// <summary>
        /// construction
        /// </summary>
        public EventsController(IEventDataAccess dal, IEventQueueProcessor eventProcessor, IAuth auth)
        {
            _dal = dal;
            _eventProcessor = eventProcessor;
            _auth = auth;
        }

        /// <summary>
        /// Get all valid event categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                return Ok(Enum.GetValues(typeof(EventCategory)).Cast<EventCategory>().Select(e => e.ToString()).ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Get all event source values
        /// </summary>
        /// <remarks>
        /// This is per-organisation
        /// </remarks>
        /// <returns></returns>
        [HttpGet("sources")]
        public async Task<IActionResult> GetSources()
        {
            try
            {
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }


                var sources = await _dal.GetSources(org);

                return Ok(sources);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Get all tags
        /// </summary>
        /// <remarks>
        /// Enumerated all the tags used by this organisation
        /// </remarks>
        /// <returns></returns>
		[HttpGet("tags")]
		public async Task<IActionResult> GetTags()
		{
			try
			{
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                var tags = await _dal.GetTags(org);

				return Ok(tags);
			}
			catch (Exception ex)
			{
				Log.Error(ex, ex.Message);

				return this.InternalServerError(ex);
			}
		}


        /// <summary>
        /// Event search
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] EventSearchCriteria criteria)
        {
            try
            {
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                var events = await _dal.SearchAsync(org, criteria);

                return Ok(events);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }


		/// <summary>
		/// Retrieves a specific event by unique id
		/// </summary>
		/// <remarks>EventDetail</remarks>
		/// <response code="200">Event found</response>
		/// <response code="400">Event has missing/invalid values</response>
		/// <response code="500">Oops! Can't find your event right now</response>
		[HttpGet("{id}", Name = "EventDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                var evt = await _dal.ReadAsync(org, id);

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


        /// <summary>
        /// Log an event
        /// </summary>
        /// <remarks>
        /// Event is inspected and processed offline
        /// </remarks>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost]
		public async Task<IActionResult> Post([FromBody] Event e)
		{
			try
			{
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                if (e == null)
                {
                    return BadRequest();
                }


                var evt = await CreateEventAsync(org, e);

                _eventProcessor.Enqueue(evt.Id);

                return CreatedAtRoute("EventDetails", new { id = evt.Id }, evt);
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


        /// <summary>
        /// Submit a batch of events
        /// </summary>
        /// <param name="evts"></param>
        /// <returns></returns>
        [HttpPost("bulk")]
        public async Task<IActionResult> PostBulk([FromBody] IEnumerable<Event> evts)
        {
            try
            {
                var apiKey = Request.ApiKey();

                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if (org == null)
                {
                    return Unauthorized();
                }

                if (evts == null)
                {
                    return BadRequest();
                }

                Parallel.ForEach(evts, async e =>
                {
                    try
                    {
                        var evt = await CreateEventAsync(org, e);

                        lock (_eventProcessor)
                        {
                            _eventProcessor.Enqueue(evt.Id);
                        }
                    }
                    catch (Exception ex)
                    {
						Log.Error(ex, ex.Message);
                    }
                });

                return Ok();
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


        /// <summary>
        /// Create event
        /// </summary>
        private async Task<EventDetails> CreateEventAsync(Organisation org, Event e)
        {
            var evt = Common.Convert.ToEventDetails(e);

            if (string.IsNullOrEmpty(evt.Source))
            {
                evt.Source = org.Name;
            }

            if (evt.Properties == null)
            {
                evt.Properties = new List<Property>();
            }

            evt.Properties.AddRange(Request.CommonProperties());

            evt.Id = await _dal.CreateAsync(org, evt);

            return evt;
        }
    }
}
