using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class EventsRepository : IEventsRepository
    {
        private readonly EventsContext _context;

        public EventsRepository(EventsContext context)
        {
            _context = context;
        }

        public Task<Event> LoadAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Event> LoadAsync(Guid reference)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Event e)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> SearchAsync()
        {
            throw new NotImplementedException();
        }
    }
}
