using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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


        public async Task SaveAsync(Event e)
        {
            var entity = new EventEntity()
            {
                Reference = e.Id == Guid.Empty ? Guid.NewGuid() : e.Id,
                TimestampUtc = e.TimestampUtc,
                Summary = e.Summary,
                Category = _context.Categories.Single(c => c.Name == e.Category.ToString().ToLowerInvariant()),
                Source = _context.Sources.Single(c => c.Name == e.Source)
            };

            foreach (var p in e.Properties) 
            {
                entity.Properties.Add(new EventPropertyEntity() { 
                    Category = p.Category,
                    Name = p.Name,
                    Value = p.Value
                });
            }

            _context.Events.Add(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Event>> SearchAsync()
        {
            var events = await _context.Events
                .Include(f => f.Source)
                .Include(f => f.Category)
                .Include(f => f.Properties)
                .ToArrayAsync();

            return events.Select(e => new Event() { 
                Id = e.Reference,
                Category = (Category)Enum.Parse(typeof(Category), e.Category.Name.ToLower()),
                Summary = e.Summary,
                TimestampUtc = e.TimestampUtc,
                Properties = e.Properties.Select(p => new EventProperty() {
                    Category = p.Category,
                    Name = p.Name,
                    Value = p.Value
                }).ToArray()                
            });
        }
    }
}
