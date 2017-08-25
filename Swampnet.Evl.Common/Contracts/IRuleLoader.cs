using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    // Jeez, I dunno, just something to abstract away the loading of rules. Name may very well change...
    public interface IRuleLoader
    {
        /// <summary>
        /// Load all the rules for this aqpplication
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        IEnumerable<Rule> Load(Application app);
    }
}
