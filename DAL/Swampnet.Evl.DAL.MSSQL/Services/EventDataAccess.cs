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
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class EventDataAccess : IEventDataAccess
    {
		private static readonly char[] _splitTags = new[] { ',' };
        private static readonly char[] _splitProperty = new[] { ':' };
        private readonly IConfiguration _cfg;

        public EventDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }

		public async Task<EventDetails> CreateAsync(Guid orgid, Event evt)
        {
            using(var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var internalEvent = Convert.ToEvent(orgid, evt, context);

                if(internalEvent.Summary.Length > 8000)
                {
                    var fullSummary = internalEvent.Summary;
                    // TODO: Put remainder of summary in properties
                    internalEvent.Summary = internalEvent.Summary.Truncate(8000, true);
                }

                if (string.IsNullOrEmpty(internalEvent.Source))
                {
                    evt.Source = internalEvent.Organisation.Name;
                }

                internalEvent.Id = Guid.NewGuid();

                context.Events.Add(internalEvent);

                await context.SaveChangesAsync();

                return Convert.ToEventDetails(internalEvent);
            }
        }


        public async Task<EventDetails> ReadAsync(Organisation org, Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var query = context.Events
                    .Include(e => e.Properties)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
                    .Include(e => e.Triggers)
                        .ThenInclude(t => t.Actions)
                            .ThenInclude(t => t.InternalActionProperties)
                                .ThenInclude(t => t.Property)
                    .Include(e => e.Organisation)
                        .ThenInclude(o => o.InternalOrganisationProperties)
                            .ThenInclude(p => p.Property)
                    .Where(e => e.Id == id);

                if(org != null)
                {
                    query = query.Where(e => e.OrganisationId == org.Id);
                }
                //var sql = query.ToSql();

                var evt = await query.SingleOrDefaultAsync();

                return Convert.ToEventDetails(evt);
            }
        }


        public async Task UpdateAsync(Organisation org, EventDetails evt)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var query = context.Events
                    .Include(e => e.Properties)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
                    .Include(e => e.Triggers)
                        .ThenInclude(t => t.Actions)
                        .ThenInclude(t => t.InternalActionProperties)
                        .ThenInclude(t => t.Property)
                    .Where(e => e.Id == evt.Id);

                var internalEvent = await query.SingleOrDefaultAsync();

                if (internalEvent == null)
                {
                    throw new NullReferenceException($"Event {evt.Id} not found");
                }

                internalEvent.Category = evt.Category.ToString();
                internalEvent.Summary = evt.Summary;
                internalEvent.ModifiedOnUtc = DateTime.UtcNow;

                internalEvent.AddProperties(evt.Properties);
                internalEvent.AddTags(context, evt.Tags, evt.Organisation.Id);
				internalEvent.AddTriggers(evt.Triggers);
                // @TODO: We should be able to remove tags that don't exist as well

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync(Organisation org, EventSearchCriteria criteria)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var query = context.Events
                    .Include(f => f.InternalEventTags)
                        .ThenInclude(f => f.Tag)
                    .AsQueryable();

                query = query.Where(e => e.OrganisationId == org.Id);

                if (criteria.Id.HasValue)
                {
                    query = query.Where(e => e.Id == criteria.Id);
                }

                if (!string.IsNullOrEmpty(criteria.Summary))
                {
                    // Either the actual summary contains the value, or a property called 'summary' contains the value.
                    // This currently kills the search. I suspect any 'property' search will do the same. Looks ok in the profiler (as in, no missing index) but we might need
                    // to play about with full-text indexes which I don't think is available on the tier I have this running on in Azure atm.
                    //query = query.Where(e =>
                    //    e.Summary.Contains(criteria.Summary)
                    //    || e.InternalEventProperties.Any(p => p.Property.Name == "Summary" && p.Property.Value.Contains(criteria.Summary)));

                    query = query.Where(e => e.Summary.Contains(criteria.Summary));
                }

                if (!string.IsNullOrEmpty(criteria.Categories))
                {
                    query = query.Where(e => criteria.Categories.Contains(e.Category));
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


                // Properties
                // name1:value1,name2:value2
                if (!string.IsNullOrEmpty(criteria.Properties))
                {
                    foreach(var part in criteria.Properties.Split(_splitTags, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var x = part.Split(_splitProperty, StringSplitOptions.RemoveEmptyEntries);
                        if(x.Length > 0)
                        {
                            var name = x[0].Trim();
                            var value = x.Length > 1 ? x[1].Trim() : "";

                            query = query.Where(e => e.Properties.Any(p => p.Name == name && p.Value.Contains(value)));
                        }
                    }
                }


                // Realtime search
                if (criteria.TimestampUtc.HasValue)
                {
                    query = query.Where(e => e.ModifiedOnUtc >= criteria.TimestampUtc);
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
				}

				query = query.OrderByDescending(e => e.TimestampUtc);

				if (criteria.PageSize > 0 && criteria.Page >= 0)
				{
					query = query.Skip(criteria.PageSize * criteria.Page).Take(criteria.PageSize);
				}

                var sql = query.ToSql();
				var results = await query.ToArrayAsync();

                return results.Select(Convert.ToEventSummary);
            }
        }

        public async Task<IEnumerable<string>> GetSources(Organisation org)
        {
            IEnumerable<string> sources = null;

            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
				var query = context.Events.AsQueryable();

                query = query.Where(e => e.OrganisationId == org.Id);

                sources = await query
                    .Select(e => e.Source)
                    .Distinct()
                    .ToListAsync();
            }

            return sources;
        }


		public async Task<IEnumerable<string>> GetTags(Organisation org)
		{
			IEnumerable<string> tags = null;

			using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
			{
				var query = context.Tags.AsQueryable();

                query = query.Where(t => t.OrganisationId == org.Id);

                tags = await query.Select(t => t.Name).Distinct().ToListAsync();
			}

			return tags;
		}


		public async Task<long> GetTotalEventCountAsync(Organisation org)
        {
            long count = 0;

            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
				var query = context.Events.AsQueryable();

                query = query.Where(e => e.OrganisationId == org.Id);

                count = await query.LongCountAsync();
            }

            return count;
        }


        public async Task TruncateEventsAsync()
        {
            using(var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                TimeSpan ts;
                if (!TimeSpan.TryParse(_cfg["evl:trunc-events-timeout"], out ts))
                {
                    ts = TimeSpan.FromSeconds(60);
                }

                context.Database.SetCommandTimeout(ts);

                await context.Database.ExecuteSqlCommandAsync("exec [evl].[TrucateExpiredEventData]");
            }
        }
    }
}
