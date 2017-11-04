using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    // Jeez, I dunno, just something to abstract away the loading of rules. Name may very well change...
    public interface IRuleDataAccess
    {
        Task<IEnumerable<RuleSummary>> SearchAsync(Organisation org);

        Task<Rule> LoadAsync(Organisation org, Guid id);

        Task<IEnumerable<Rule>> LoadAsync(Organisation org);

        Task CreateAsync(Profile profile, Rule rule);

        Task UpdateAsync(Profile profile, Rule rule);

        Task DeleteAsync(Profile profile, Guid id);

        Task ReorderAsync(Profile profile, IEnumerable<RuleOrder> rules);
    }
}
