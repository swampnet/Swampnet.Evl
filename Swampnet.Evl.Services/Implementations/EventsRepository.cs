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
        private IEnumerable<CategoryEntity> _categories;

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
                Category = await ResolveCategoryAsync(e.Category),
                Source = await ResolveSourceAsync(e.Source)
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
                Source = e.Source.Name,
                Properties = e.Properties.Select(p => new EventProperty() {
                    Category = p.Category,
                    Name = p.Name,
                    Value = p.Value
                }).ToArray()
            });
        }


        private async Task<CategoryEntity> ResolveCategoryAsync(Category category)
        {
            if(_categories == null)
            {
                _categories = await _context.Categories.ToArrayAsync();
            }
            return _categories.Single(c => c.Name.Equals(category.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

        private static object _lock = new object();

        private Task<SourceEntity> ResolveSourceAsync(string source)
        {
            var entity = _context.Sources.Local.SingleOrDefault(s => s.Name == source);

            if (entity == null)
            {
                lock (_lock)
                {
                    entity = _context.Sources.Local.SingleOrDefault(s => s.Name == source);
                    if(entity == null)
                    {
                        entity = new SourceEntity()
                        {
                            Name = source
                        };

                        _context.Sources.Add(entity);
                    }
                }
            }

            return Task.FromResult(entity);
        }
    }
}
