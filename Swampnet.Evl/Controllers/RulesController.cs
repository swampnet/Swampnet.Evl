using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Threading.Tasks;

namespace Swampnet.Evl.Controllers
{
    [Route("api/rules")]
    public class RulesController : Controller
    {
        private readonly IRuleDataAccess _rulesData;

        public RulesController(IRuleDataAccess rulesData)
        {
            _rulesData = rulesData;
        }

        
        // GET api/rules
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // @TODO: Auth
            // @TODO: Just want rules that we're allowed to see.
            var rules = await _rulesData.SearchAsync();

            return Ok(rules);
        }


        // GET api/rules/<id>
        [HttpGet("{id}", Name = "RuleDetails")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rule = await _rulesData.LoadAsync(id);

            return Ok(rule);
        }


        // POST api/rules
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rule rule)
        {
            Log.Information("Create rule {ruleName}", rule.Name);
            
            // @TODO: Auth

            await _rulesData.CreateAsync(rule);

            return CreatedAtRoute("RuleDetails", new { id = rule.Id }, rule);
        }


        // PUT api/rules/<id>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Rule rule)
        {
            try
            {
                Log.Information("Put rule {ruleId} {ruleName}", id, rule.Name);

                // @TODO: Auth

                if (rule.Id.HasValue && rule.Id.Value != id)
                {
                    // @TODO: Something off here: We might be trying to update the wrong rule.
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
                throw;
            }
        }


        // DELETE api/rules/<id>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Delay(1);

            Log.Information("Delete rule {ruleId}", id);

            // @TODO: Auth
            // @TODO: Delete

            return Ok();
        }
    }
}
