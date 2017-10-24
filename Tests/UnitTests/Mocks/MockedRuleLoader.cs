using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    internal class MockedRuleLoader : IRuleDataAccess
    {
        private readonly IEnumerable<Rule> _rules;

        public MockedRuleLoader(IEnumerable<Rule> rules)
        {
            _rules = rules;
        }

        public Task CreateAsync(Organisation org, Rule rule)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Organisation org, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Rule> LoadAsync(Organisation org, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rule>> LoadAsync(Organisation org)
        {
            return Task.Run(() => _rules);
        }

        public Task<IEnumerable<RuleSummary>> SearchAsync(Organisation org)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Organisation org, Rule rule)
        {
            throw new NotImplementedException();
        }
    }
}
