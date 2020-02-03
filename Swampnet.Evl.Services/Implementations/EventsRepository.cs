using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Swampnet.Evl.Services.Implementations
{
    class EventsRepository : IEventsRepository
    {
        private readonly EventsContext _context;
        private IEnumerable<CategoryEntity> _categories;
        private readonly ITagRepository _tags;
        private readonly ISourceRepository _sourceService;

        public EventsRepository(EventsContext context, ITagRepository tags, ISourceRepository sourceService)
        {
            _context = context;
            _tags = tags;
            _sourceService = sourceService;
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
                SourceId = (await _sourceService.ResolveAsync(e.Source)).Id
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

        private static char[] _tagSplit = new[] { ',', ';' };

        public async Task<EventSearchResult> SearchAsync(EventSearchCriteria criteria)
        {
            var sw = Stopwatch.StartNew();
            var rs = new EventSearchResult();


            var events = _context.Events
                .Include(f => f.Source)
                .Include(f => f.Category)
                .Include(f => f.History)
                .Include(f => f.EventTags)
                    .ThenInclude(f => f.Tag)
                .AsQueryable();

            if(criteria.Id != null)
            {
                events = events.Where(e => e.Reference == criteria.Id);
            }
            if (!string.IsNullOrEmpty(criteria.Summary))
            {
                events = events.Where(e => e.Summary.Contains(criteria.Summary));
            }
            if (criteria.Start.HasValue)
            {
                events = events.Where(e => e.TimestampUtc >= criteria.Start);
            }
            if (criteria.End.HasValue)
            {
                events = events.Where(e => e.TimestampUtc <= criteria.End);
            }
            if (criteria.Category.HasValue)
            {
                var cat = criteria.Category.ToString().ToLowerInvariant();
                events = events.Where(e => e.Category.Name == cat);
            }
            if(!string.IsNullOrEmpty(criteria.Source))
            {
                events = events.Where(e => e.Source.Name == criteria.Source);
            }
            if (!string.IsNullOrEmpty(criteria.Tags))
            {
                var tags = criteria.Tags.Split(_tagSplit, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());
                foreach(var tag in tags)
                {
                    events = events.Where(e => e.EventTags.Any(et => et.Tag.Name == tag));
                }
            }

            rs.TotalCount = await events.CountAsync();

            events = events.OrderByDescending(e => e.TimestampUtc);

            // Paging
            criteria.Page = criteria.Page == 0 ? 1 : criteria.Page;
            if(Math.Ceiling((double)rs.TotalCount / criteria.PageSize) < criteria.Page)
            {
                criteria.Page = (int)Math.Ceiling((double)rs.TotalCount / criteria.PageSize);
            }

            events = events.Skip((criteria.Page-1) * criteria.PageSize).Take(criteria.PageSize);

            var results = await events.ToArrayAsync();

            rs.Events = results.Select(e => new EventSummary()
            {
                Id = e.Reference,
                Category = (Category)Enum.Parse(typeof(Category), e.Category.Name.ToLower()),
                Summary = e.Summary,
                TimestampUtc = e.TimestampUtc,
                Source = e.Source.Name,
                Tags = e.EventTags.Select(et => et.Tag.Name).ToList()
            }).ToArray();

            rs.Elapsed = sw.Elapsed;
            rs.Page = criteria.Page;
            rs.TotalPages = (int)Math.Ceiling((double)rs.TotalCount / criteria.PageSize);
            rs.PageSize = criteria.PageSize;

            return rs;
        }


        private async Task<CategoryEntity> ResolveCategoryAsync(Category category)
        {
            if(_categories == null)
            {
                _categories = await _context.Categories.ToArrayAsync();
            }
            return _categories.Single(c => c.Name.Equals(category.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
