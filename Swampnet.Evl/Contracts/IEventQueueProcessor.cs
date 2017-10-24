using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Contracts
{
    /// <summary>
    /// Event queue processor
    /// </summary>
    /// <remarks>
    /// When a new event arrives we return straight away to the client, but queue up the event for offline processing.
    /// </remarks>
    public interface IEventQueueProcessor
    {
        /// <summary>
        /// Add a new event to the queue
        /// </summary>
        /// <param name="id"></param>
        void Enqueue(Guid id);

        /// <summary>
        /// Add multiple events to the queue
        /// </summary>
        /// <param name="ids"></param>
        void Enqueue(IEnumerable<Guid> ids);
    }
}
