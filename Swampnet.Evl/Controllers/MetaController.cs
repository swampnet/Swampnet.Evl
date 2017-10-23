using Microsoft.AspNetCore.Mvc;
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
    /// Meta data
    /// </summary>
    [Route("meta")]
    public class MetaController : Controller
    {
        private readonly IEventDataAccess _eventDataAccess;
        private readonly IRuleDataAccess _ruleDataAccess;
        private readonly IEnumerable<IActionHandler> _actionHandlers;
        private readonly IAuth _auth;

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="eventDataAccess"></param>
        /// <param name="ruleDataAccess"></param>
        /// <param name="actionHandlers"></param>
        /// <param name="auth"></param>
        public MetaController(
            IEventDataAccess eventDataAccess,
            IRuleDataAccess ruleDataAccess,
            IEnumerable<IActionHandler> actionHandlers,
            IAuth auth)
        {
            _eventDataAccess = eventDataAccess;
            _ruleDataAccess = ruleDataAccess;
            _actionHandlers = actionHandlers;
            _auth = auth;
        }


        /// <summary>
        /// GET meta
        /// </summary>
        /// <returns>MetaData</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var org = await _auth.GetOrganisationByApiKeyAsync(Common.Constants.MOCKED_DEFAULT_APIKEY);
            var metaData = await GetMetaData(org);

            return Ok(metaData);
        }


        private async Task<MetaData> GetMetaData(Organisation org)
        {
            return new MetaData()
            {
                ActionMetaData = await GetActionMetaData(org),
                Operands = await GetOperands(org),
                Operators = await GetOperators(org)
            };
        }


        private Task<ExpressionOperator[]> GetOperators(Organisation org)
        {
            return Task.FromResult(_operators);
        }


        private async Task<MetaDataCapture[]> GetOperands(Organisation org)
        {
            var sources = await _eventDataAccess.GetSources(org);

            // Start of with our static list
            var operands = new List<MetaDataCapture>(_operands);

            // Add in dynamic source
            operands.Add(new MetaDataCapture()
            {
                Name = "Source",
                DataType = "select",
                Options = sources.Select(s => new Option(s)).ToArray()
            });

            return operands.ToArray();
        }


        private Task<ActionMetaData[]> GetActionMetaData(Organisation org)
        {
            return Task.FromResult(
                _actionHandlers
                .Select(a => new ActionMetaData()
                {
                    Type = a.Type,
                    Properties = a.GetPropertyMetaData()
                })
                .ToArray());
        }


        #region Static data
        private static readonly ExpressionOperator[] _operators = new[]
        {
            new ExpressionOperator(RuleOperatorType.MATCH_ALL, "Match All"),
            new ExpressionOperator(RuleOperatorType.MATCH_ANY, "Match Any"),

            new ExpressionOperator(RuleOperatorType.EQ, "="),
            new ExpressionOperator(RuleOperatorType.NOT_EQ, "<>"),
            new ExpressionOperator(RuleOperatorType.GT, ">"),
            new ExpressionOperator(RuleOperatorType.GTE, ">="),
            new ExpressionOperator(RuleOperatorType.LT, "<"),
            new ExpressionOperator(RuleOperatorType.LTE, "<="),
            new ExpressionOperator(RuleOperatorType.REGEX, "Match Expression"),

            new ExpressionOperator(RuleOperatorType.TAGGED, "Is Tagged"),
            new ExpressionOperator(RuleOperatorType.NOT_TAGGED, "Is NOT Tagged")
        };


        private static readonly MetaDataCapture[] _operands = new[]
        {
            new MetaDataCapture()
            {
                Name = "Summary"
            },

            new MetaDataCapture()
            {
                Name = "Timestamp",
                DataType = "datetime"
            },

            new MetaDataCapture()
            {
                Name = "Category",
                DataType = "select",
                Options = new[]
                {
                    new Option("Information", "Information"),
                    new Option("Error", "Error"),
                    new Option("Debug", "Debug")
                }
            },

            new MetaDataCapture()
            {
                Name = "Property",
                DataType = "require-args"
            }
        };

        #endregion
    }
}
