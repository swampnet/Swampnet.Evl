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
        private ITags _tags;

        public EventsRepository(EventsContext context, ITags tags)
        {
            _context = context;
            _tags = tags;
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

            if (e.Properties != null)
            {
                foreach (var p in e.Properties)
                {
                    entity.Properties.Add(new EventPropertyEntity()
                    {
                        Category = p.Category,
                        Name = p.Name,
                        Value = p.Value
                    });
                }
            }

            if (e.History != null)
            {
                foreach (var h in e.History)
                {
                    entity.History.Add(new EventHistoryEntity()
                    {
                        TimestampUtc = h.TimestampUtc,
                        Type = h.Type,
                        Details = h.Details
                    });
                }
            }

            if(e.Tags != null)
            {
                foreach(var t in e.Tags.Select(t => t.ToLower().Trim()).Distinct())
                {
                    var tag = await _tags.ResolveAsync(t);
                    entity.EventTags.Add(new EventTagsEntity() { 
                        TagId = tag.Id
                    });
                }
            }

            _context.Events.Add(entity);

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync()
        {
            var events = await _context.Events
                .Include(f => f.Source)
                .Include(f => f.Category)
                .Include(f => f.History)
                .Include(f => f.EventTags)
                    .ThenInclude(f => f.Tag)
                .ToArrayAsync();

            return events.Select(e => new EventSummary()
            {
                Id = e.Reference,
                Category = (Category)Enum.Parse(typeof(Category), e.Category.Name.ToLower()),
                Summary = e.Summary,
                TimestampUtc = e.TimestampUtc,
                Source = e.Source.Name,
                Tags = e.EventTags.Select(et => et.Tag.Name).ToList()
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


        private async Task<SourceEntity> ResolveSourceAsync(string source)
        {
            // So, we're *definately* create dups here if we have a new source and have many concurrent requests with that source coming in.
            var entity = await _context.Sources.SingleOrDefaultAsync(s => s.Name == source);

            if (entity == null)
            {
                entity = new SourceEntity()
                {
                    Name = source
                };

                _context.Sources.Add(entity);
            }
            
            return entity;
        }


        private async Task<TagEntity> ResolveTagAsync(string tag)
        {
            // So, we're *definately* create dups here if we have a new tag and have many concurrent requests with that source coming in.
            var entity = await _context.Tags.SingleOrDefaultAsync(s => s.Name == tag);

            if (entity == null)
            {
                entity = new TagEntity()
                {
                    Name = tag
                };

                _context.Tags.Add(entity);
            }

            return entity;
        }

    }
}
