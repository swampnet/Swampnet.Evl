using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    /// <summary>
    /// All things rule based
    /// </summary>
    [Route("rules")]
    public class RulesController : Controller
    {
        private readonly IRuleDataAccess _rulesData;
		private readonly IAuth _auth;

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="rulesData"></param>
		public RulesController(IAuth auth, IRuleDataAccess rulesData)
        {
			_auth = auth;
            _rulesData = rulesData;
        }

        
        /// <summary>
        /// Get all rules for the current organisation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_view))
                {
                    return Unauthorized();
                }

                var rules = await _rulesData.SearchAsync(profile.Organisation);

                return Ok(rules);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Get rule details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "RuleDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_view))
                {
                    return Unauthorized();
                }

                var rule = await _rulesData.LoadAsync(profile.Organisation, id);

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


        /// <summary>
        /// Create a new rule
        /// </summary>
        /// <remarks>
        /// POST /rules
        /// </remarks>
        /// <param name="rule"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rule rule)
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_create))
                {
                    return Unauthorized();
                }

                if (rule == null)
				{
					return BadRequest();
				}

				Log.Debug("POST rule {ruleName}", rule.Name);

				await _rulesData.CreateAsync(profile, rule);

                return CreatedAtRoute("RuleDetails", new { id = rule.Id }, rule);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }


        /// <summary>
        /// Reorder rules.
        /// 
        /// We're slipping into command/query here, where 'reorder' is the command. Not sure if the REST
        /// guys will kill me for this sort of stuff.
        /// </summary>
        /// <remarks>
        /// POST /rules/reorder
        /// </remarks>
        /// <param name="rules"></param>
        /// <returns></returns>
        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] IEnumerable<RuleOrder> rules)
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_edit))
                {
                    return Unauthorized();
                }

                await _rulesData.ReorderAsync(profile, rules);

                var reordered = await _rulesData.SearchAsync(profile.Organisation);

                return Ok(reordered);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update an existing rule
        /// 
        /// PUT rules/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Rule rule)
        {
            try
            {
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_edit))
                {
                    return Unauthorized();
                }

                if (rule == null)
				{
					return BadRequest();
				}

				Log.Debug("PUT rule {ruleId} {ruleName}", id, rule.Name);

				// Something off here: We might be trying to update the wrong rule.
				if (rule.Id.HasValue && rule.Id.Value != id)
                {
					return BadRequest("id and Rule.Id do not match");
				}

				await _rulesData.UpdateAsync(profile, rule);

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


        /// <summary>
        /// Delete a rule
        /// 
        /// DELETE rules/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
			try
			{
                var profile = await _auth.GetProfileAsync(User);
                if (profile == null || !profile.HasPermission(Permission.rule_delete))
                {
                    return Unauthorized();
                }

                Log.Information("DEL rule {ruleId}", id);

				await _rulesData.DeleteAsync(profile, id);

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
