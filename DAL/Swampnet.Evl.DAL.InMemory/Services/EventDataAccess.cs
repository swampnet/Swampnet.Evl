using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.DAL.InMemory.Services
{
    class EventDataAccess : IEventDataAccess
    {
		private static readonly char[] _splitTags = new[] { ',' };


		public async Task<Guid> CreateAsync(Organisation org, EventDetails evt)
        {
            using(var context = EventContext.Create())
            {
                var internalEvent = Convert.ToInternalEvent(evt, context);
                internalEvent.Id = Guid.NewGuid();
                context.Events.Add(internalEvent);

                await context.SaveChangesAsync();

                return internalEvent.Id;
            }
        }

        public async Task<EventDetails> ReadAsync(Organisation org, Guid id)
        {
            using (var context = EventContext.Create())
            {
                var evt = await context.Events
                    .Include(e => e.InternalEventProperties)
                        .ThenInclude(p => p.Property)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
                    .SingleOrDefaultAsync(e => e.Id == id);

                return Convert.ToEvent(evt);
            }
        }


        public async Task UpdateAsync(Organisation org, Guid id, EventDetails evt)
        {
            using (var context = EventContext.Create())
            {
                var internalEvent = await context.Events
                    .Include(e => e.InternalEventProperties)
                        .ThenInclude(p => p.Property)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (internalEvent == null)
                {
                    throw new NullReferenceException($"Event {id} not found");
                }

                internalEvent.Category = evt.Category.ToString();
                internalEvent.Summary = evt.Summary;
                internalEvent.LastUpdatedUtc = DateTime.UtcNow;

                // Update properties. At the moment we can only add new properties (How would we event match and update changed values? We can have multiple
                // properties with the same category / name.
                //foreach (var prp in evt.Properties)
                //{
                //    if (!internalEvent.Properties.Any(p => p.Category.EqualsNoCase(prp.Category) && p.Name.EqualsNoCase(prp.Name) && p.Value.EqualsNoCase(prp.Value)))
                //    {
                //        internalEvent.Properties.Add(Convert.ToInternalProperty(prp));
                //    }
                //}
                internalEvent.AddProperties(evt.Properties);

                // Add tags, will ignore any that already exist so we'll only add new tags
                internalEvent.AddTags(context, evt.Tags);

                // @TODO: We should be able to remove tags that don't exist as well

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync(Organisation org, EventSearchCriteria criteria)
        {
            using (var context = EventContext.Create())
            {
                var query = context.Events.AsQueryable();

                if (criteria.Id.HasValue)
                {
                    query = query.Where(e => e.Id == criteria.Id);
                }

                if (!string.IsNullOrEmpty(criteria.Summary))
                {
                    query = query.Where(e => e.Summary.Contains(criteria.Summary));
                }

                if (criteria.Category.HasValue)
                {
                    query = query.Where(e => e.Category == criteria.Category.ToString());
                }

                if (!string.IsNullOrEmpty(criteria.Source))
                {
                    query = query.Where(e => e.Source == criteria.Source);

                    // Version must match exactly
                    if (!string.IsNullOrEmpty(criteria.SourceVersion))
                    {
                        query = query.Where(e => e.SourceVersion == criteria.SourceVersion);
                    }
                }

                if(!string.IsNullOrEmpty(criteria.Tags))
                {
                    foreach(var tag in criteria.Tags.Split(_splitTags, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Where(e => e.InternalEventTags.Any(t => t.Tag.Name == tag.Trim()));
                    }
                }


                // Realtime search
                if (criteria.TimestampUtc.HasValue)
                {
                    query = query.Where(e => e.LastUpdatedUtc >= criteria.TimestampUtc);
                }

                // Advanced search
                else
                {
                    if (criteria.FromUtc.HasValue)
                    {
                        query = query.Where(e => e.TimestampUtc >= criteria.FromUtc);
                    }

                    if (criteria.ToUtc.HasValue)
                    {
                        query = query.Where(e => e.TimestampUtc <= criteria.ToUtc);
                    }

					// Advanced search returns in date order (oldest first) - Is this going to be confusing?
				}

				query = query.OrderByDescending(e => e.TimestampUtc);

				if (criteria.PageSize > 0 && criteria.Page >= 0)
				{
					query = query.Skip(criteria.PageSize * criteria.Page).Take(criteria.PageSize);
				}

				var results = await query.ToArrayAsync();

                return results.Select(e => Convert.ToEventSummary(e));
            }
        }

        public async Task<IEnumerable<string>> GetSources(Organisation org)
        {
            IEnumerable<string> sources = null;

            using (var context = EventContext.Create())
            {
                sources = await context.Events.Select(e => e.Source).Distinct().ToListAsync();
            }

            return sources;
        }

		public async Task<IEnumerable<string>> GetTags(Organisation org)
		{
			IEnumerable<string> tags = null;

			using (var context = EventContext.Create())
			{
				tags = await context.Tags.Select(t => t.Name).Distinct().ToListAsync();
			}

			return tags;
		}

		public async Task<long> GetTotalEventCountAsync(Organisation org)
        {
            long count = 0;

            using (var context = EventContext.Create())
            {
                count = await context.Events.LongCountAsync();
            }

            return count;
        }
    }
}
