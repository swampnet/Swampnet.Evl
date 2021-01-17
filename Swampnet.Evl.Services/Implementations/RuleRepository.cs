using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class RuleRepository : IRuleRepository
    {
        private readonly EventsContext _context;

        public RuleRepository(EventsContext context)
        {
            _context = context;
        }


        public async Task<Rule[]> LoadRulesAsync()
        {
            var source = await _context.Rules.ToArrayAsync();

            return source.Select(r => new Rule()
            {
                Id = r.Id,
                Priority = r.Priority,
                IsEnabled = r.IsEnabled,
                Name = r.Name,
                Expression = r.Expression.DeserializeXml<Expression>(),
                Actions = r.Actions.DeserializeXml<ActionDefinition[]>()
            }).ToArray();
        }
    }
}
