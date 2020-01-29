using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Swampnet.Evl.Services.Implementations
{
    class ProcessService : IProcess
    {
        private readonly IEnumerable<IEventProcessor> _eventProcessors;
        private readonly EventsContext _context;

        public ProcessService(IEnumerable<IEventProcessor> eventProcessors, EventsContext context)
        {
            _eventProcessors = eventProcessors;
            _context = context;
        }


        public async Task ProcessEventAsync(Guid id)
        {
            var e = await _context.Events
                .Include(f => f.Category)
                .Include(f => f.Source)
                .Include(f => f.Properties)
                .SingleAsync(x => x.Reference == id);

            foreach(var p in _eventProcessors.OrderBy(x => x.Priority))
            {
                await p.ProcessAsync(e);

                e.ModifiedOnUtc = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}
