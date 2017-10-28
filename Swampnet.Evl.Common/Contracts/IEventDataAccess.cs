using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IEventDataAccess
    {
        /// <summary>
        /// Save a new event to the backing store
        /// </summary>
        /// <param name="app"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task<Guid> CreateAsync(Organisation org, EventDetails evt);

        /// <summary>
        /// Read an existing event from the backing store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EventDetails> ReadAsync(Organisation org, Guid id);

        /// <summary>
        /// Update an existing event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task UpdateAsync(Organisation org, Guid id, EventDetails evt);

        Task<IEnumerable<EventSummary>> SearchAsync(Profile profile, EventSearchCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetSources(Profile profile);
		Task<IEnumerable<string>> GetTags(Profile profile);
		Task<long> GetTotalEventCountAsync(Profile profile);
    }
}
