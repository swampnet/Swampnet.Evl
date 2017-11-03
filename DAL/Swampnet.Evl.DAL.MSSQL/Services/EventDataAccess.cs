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
        private readonly IConfiguration _cfg;

        public EventDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }

		public async Task<Guid> CreateAsync(Organisation org, EventDetails evt)
        {
            using(var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var internalEvent = Convert.ToEvent(org, evt, context);

                internalEvent.Id = Guid.NewGuid();

                context.Events.Add(internalEvent);

                await context.SaveChangesAsync();

                return internalEvent.Id;
            }
        }

        // @TODO: We're not using org.id atm - reason being that for some internal operations we don't have an organisation
        public async Task<EventDetails> ReadAsync(Organisation org, Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var evt = await context.Events
                    .Include(e => e.InternalEventProperties)
                        .ThenInclude(p => p.Property)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
					.Include(e => e.Triggers)
						.ThenInclude(t => t.Actions)
						.ThenInclude(t => t.InternalActionProperties)
						.ThenInclude(t => t.Property)
					.SingleOrDefaultAsync(e => e.Id == id);

                return Convert.ToEventDetails(evt);
            }
        }

        // @TODO: We're not using org.id atm - reason being that for some internal operations we don't have an organisation
        public async Task UpdateAsync(Organisation org, Guid id, EventDetails evt)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var internalEvent = await context.Events
                    .Include(e => e.InternalEventProperties)
                        .ThenInclude(p => p.Property)
                    .Include(e => e.InternalEventTags)
                        .ThenInclude(t => t.Tag)
					.Include(e => e.Triggers)
						.ThenInclude(t => t.Actions)
						.ThenInclude(t => t.InternalActionProperties)
						.ThenInclude(t => t.Property)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (internalEvent == null)
                {
                    throw new NullReferenceException($"Event {id} not found");
                }

                internalEvent.Category = evt.Category.ToString();
                internalEvent.Summary = evt.Summary;
                internalEvent.ModifiedOnUtc = DateTime.UtcNow;

                internalEvent.AddProperties(evt.Properties);
                internalEvent.AddTags(context, evt.Tags, org);
				internalEvent.AddTriggers(evt.Triggers);
                // @TODO: We should be able to remove tags that don't exist as well

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync(Profile profile, EventSearchCriteria criteria)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var query = context.Events.AsQueryable();

				if (!profile.HasPermission(Permission.organisation_view_all))
				{
					query = query.Where(e => e.OrganisationId == profile.Organisation.Id);
				}

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

					// Advanced search returns in date order (oldest first) - Is this going to be confusing?
				}

				query = query.OrderByDescending(e => e.TimestampUtc);

				if (criteria.PageSize > 0 && criteria.Page >= 0)
				{
					query = query.Skip(criteria.PageSize * criteria.Page).Take(criteria.PageSize);
				}

				var results = await query.ToArrayAsync();

                return results.Select(Convert.ToEventSummary);
            }
        }

        public async Task<IEnumerable<string>> GetSources(Profile profile)
        {
            IEnumerable<string> sources = null;

            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
				var query = context.Events.AsQueryable();

				if (!profile.HasPermission(Permission.organisation_view_all))
				{
					query = query.Where(e => e.OrganisationId == profile.Organisation.Id);
				}

				sources = await query
                    .Select(e => e.Source)
                    .Distinct()
                    .ToListAsync();
            }

            return sources;
        }


		public async Task<IEnumerable<string>> GetTags(Profile profile)
		{
			IEnumerable<string> tags = null;

			using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
			{
				var query = context.Tags.AsQueryable();

				if (!profile.HasPermission(Permission.organisation_view_all))
				{
					query = query.Where(t => t.OrganisationId == profile.Organisation.Id);
				}


				tags = await query.Select(t => t.Name).Distinct().ToListAsync();
			}

			return tags;
		}


		public async Task<long> GetTotalEventCountAsync(Profile profile)
        {
            long count = 0;

            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
				var query = context.Events.AsQueryable();

				if (!profile.HasPermission(Permission.organisation_view_all))
				{
					query = query.Where(e => e.OrganisationId == profile.Organisation.Id);
				}

                count = await query.LongCountAsync();
            }

            return count;
        }
    }
}
