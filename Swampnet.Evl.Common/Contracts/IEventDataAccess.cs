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
        //Task CreateAsync(Guid orgid, EventDetails evt);
        Task<EventDetails> CreateAsync(Guid orgid, Event evt);
        /// <summary>
        /// Read an existing event from the backing store
        /// </summary>
        Task<EventDetails> ReadAsync(Organisation org, Guid id);

        Task TruncateEventsAsync();

        /// <summary>
        /// Update an existing event
        /// </summary>
        Task UpdateAsync(Organisation org, EventDetails evt);

        Task<IEnumerable<EventSummary>> SearchAsync(Organisation org, EventSearchCriteria criteria);

        Task<IEnumerable<string>> GetSources(Organisation org);

        Task<IEnumerable<string>> GetTags(Organisation org);

        Task<long> GetTotalEventCountAsync(Organisation org);
    }
}
