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
        Task<IEnumerable<RuleSummary>> SearchAsync();

        Task<Rule> LoadAsync(Guid id);

        Task<IEnumerable<Rule>> LoadAsync(Application app);

        Task CreateAsync(Rule rule);

        Task UpdateAsync(Rule rule);

        Task DeleteAsync(Guid id);
    }
}
