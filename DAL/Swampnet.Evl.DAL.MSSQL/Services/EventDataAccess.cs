﻿using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Swampnet.Evl.Client;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class EventDataAccess : IEventDataAccess
    {
		private static readonly char[] _splitTags = new[] { ',' };
        private readonly IConfiguration _cfg;

        public EventDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }

		public async Task<Guid> CreateAsync(Organisation org, Event evt)
        {
            using(var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                var internalEvent = Convert.ToEvent(evt, context);
                internalEvent.Id = Guid.NewGuid();
                internalEvent.OrganisationId = org == null 
                                                ? Constants.MOCKED_DEFAULT_ORGANISATION 
                                                : org.Id;

                context.Events.Add(internalEvent);

                await context.SaveChangesAsync();

                evt.Id = internalEvent.Id;

                return internalEvent.Id;
            }
        }

        public async Task<Event> ReadAsync(Organisation org, Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
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


        public async Task UpdateAsync(Organisation org, Guid id, Event evt)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
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

                internalEvent.AddProperties(evt.Properties);
                internalEvent.AddTags(context, evt.Tags);

                // @TODO: We should be able to remove tags that don't exist as well

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync(Organisation org, EventSearchCriteria criteria)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
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

            using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                sources = await context.Events.Select(e => e.Source).Distinct().ToListAsync();
            }

            return sources;
        }

		public async Task<IEnumerable<string>> GetTags(Organisation org)
		{
			IEnumerable<string> tags = null;

			using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
			{
				tags = await context.Tags.Select(t => t.Name).Distinct().ToListAsync();
			}

			return tags;
		}

		public async Task<long> GetTotalEventCountAsync(Organisation org)
        {
            long count = 0;

            using (var context = EvlContext.Create(_cfg.GetConnectionString("dbmain")))
            {
                count = await context.Events.LongCountAsync();
            }

            return count;
        }
    }
}
