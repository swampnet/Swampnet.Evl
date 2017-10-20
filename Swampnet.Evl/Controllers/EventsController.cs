using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("events")]
	public class EventsController : Controller
	{
        private readonly IEventQueueProcessor _eventProcessor;
        private readonly IEventDataAccess _dal;
        private readonly IAuth _auth;

        public EventsController(IEventDataAccess dal, IEventQueueProcessor eventProcessor, IAuth auth)
        {
            _dal = dal;
            _eventProcessor = eventProcessor;
            _auth = auth;
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(Enum.GetValues(typeof(EventCategory)).Cast<EventCategory>().Select(e => e.ToString()).ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }


        [HttpGet("sources")]
        public async Task<IActionResult> GetSources()
        {
            try
            {
                var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);

                var sources = await _dal.GetSources(org);

                return Ok(sources);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return this.InternalServerError(ex);
            }
        }

		[HttpGet("tags")]
		public async Task<IActionResult> GetTags()
		{
			try
			{
                var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);

                var tags = await _dal.GetTags(org);

				return Ok(tags);
			}
			catch (Exception ex)
			{
				Log.Error(ex, ex.Message);

				return this.InternalServerError(ex);
			}
		}


		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] EventSearchCriteria criteria)
        {
            try
            {
                Log.Logger.WithPublicProperties(criteria).Debug("Get");

                var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);

                var events = await _dal.SearchAsync(org, criteria);

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
                var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);
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
        /// Main POST
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost]
		public async Task<IActionResult> Post([FromBody] Event e)
		{
			try
			{
                var evt = Common.Convert.ToEventDetails(e);

				if (evt == null)
				{
					return BadRequest();
				}

                //var apiKey = Request.ApiKey();
                var apiKey = Common.Constants.MOCKED_DEFAULT_APIKEY;

                // @TODO: We drive everything off the API key, we currently don't check if the caller is actually
                //        the owner of the key.

                // @TODO: Auth
                // @TODO: Check api key is valid, get organisation
                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);
                if(org == null)
                {
                    return Unauthorized();
                }

                if (string.IsNullOrEmpty(evt.Source))
                {
                    evt.Source = org.Name;
                }

				if(evt.Properties == null)
				{
					evt.Properties = new List<Property>();
				}

                evt.Properties.AddRange(Request.CommonProperties());

                evt.Id = await _dal.CreateAsync(org, evt);

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


        [HttpPost("bulk")]
        public async Task<IActionResult> PostBulk([FromBody] IEnumerable<Event> evts)
        {
            try
            {
                if (evts == null)
                {
                    return BadRequest();
                }

                var apiKey = Request.ApiKey();

                // @TODO: Auth
                var org = await _auth.GetOrganisationByApiKeyAsync(apiKey);

                Parallel.ForEach(evts, async evt =>
                {
                    try
                    {
                        evt.Properties.AddRange(Request.CommonProperties());
                        var id = await _dal.CreateAsync(org, Common.Convert.ToEventDetails(evt));
						_eventProcessor.Enqueue(id);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
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
    }
}
