using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IActionHandler
    {
        /// <summary>
        /// Action code / type
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt">The event that triggered this action</param>
        /// <param name="actionDefinition">The action definition</param>
        /// <param name="rule">The rule that triggered this action</param>
        Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        MetaDataCapture[] GetPropertyMetaData();
    }
}
