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
        Task<Guid> CreateAsync(Application app, Event evt);

        /// <summary>
        /// Read an existing event from the backing store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Event> ReadAsync(Guid id);

        /// <summary>
        /// Update an existing event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task UpdateAsync(Guid id, Event evt);


        Task<IEnumerable<EventSummary>> SearchAsync(EventSearchCriteria criteria);
    }
}
