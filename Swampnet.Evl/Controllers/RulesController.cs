using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("rules")]
    public class RulesController : Controller
    {
        private readonly IRuleDataAccess _rulesData;

        public RulesController(IRuleDataAccess rulesData)
        {
            _rulesData = rulesData;
        }

        
        // GET rules
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // @TODO: Auth
                // @TODO: Just want rules that we're allowed to see.
                var rules = await _rulesData.SearchAsync();

                return Ok(rules);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        // GET rules/<id>
        [HttpGet("{id}", Name = "RuleDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var rule = await _rulesData.LoadAsync(id);

                if(rule == null)
                {
                    return NotFound();
                }

                return Ok(rule);
            }
            catch (Exception ex)
            {
                ex.AddData("id", id);
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        // POST rules
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rule rule)
        {
            try
            {
				if(rule == null)
				{
					return BadRequest();
				}

				Log.Debug("POST rule {ruleName}", rule.Name);

                // @TODO: Auth

                await _rulesData.CreateAsync(rule);

                return CreatedAtRoute("RuleDetails", new { id = rule.Id }, rule);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        // PUT rules/<id>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Rule rule)
        {
            try
            {
				if (rule == null)
				{
					return BadRequest();
				}

				Log.Debug("PUT rule {ruleId} {ruleName}", id, rule.Name);

				// @TODO: Auth

				// Something off here: We might be trying to update the wrong rule.
				if (rule.Id.HasValue && rule.Id.Value != id)
                {
					return BadRequest("id and Rule.Id do not match");
				}

				await _rulesData.UpdateAsync(rule);

                // Not sure I should be returning this for an update?
                return CreatedAtRoute("RuleDetails", new { id = id }, rule);
            }
            catch (NullReferenceException ex) // Dangerous. Might be because of something else.
            {
                ex.AddData("id", id);
                Log.Error(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                ex.AddData("id", id);
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        // DELETE rules/<id>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
			try
			{
				Log.Information("DEL rule {ruleId}", id);

				// @TODO: Auth

				await _rulesData.DeleteAsync(id);

				return Ok();
			}
			catch (NullReferenceException ex) // Dangerous. Might be because of something else.
			{
				ex.AddData("id", id);
				Log.Error(ex, ex.Message);
				return NotFound();
			}
			catch (Exception ex)
			{
				ex.AddData("id", id);
				Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }
	}
}
