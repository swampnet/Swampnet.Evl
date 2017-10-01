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
        public async Task<Guid> CreateAsync(Application app, Event evt)
        {
            using(var context = EventContext.Create())
            {
                var internalEvent = Convert.ToInternalEvent(evt);
                internalEvent.Id = Guid.NewGuid();
                context.Events.Add(internalEvent);

                await context.SaveChangesAsync();

                evt.Id = internalEvent.Id;

                return internalEvent.Id;
            }
        }

        public async Task<Event> ReadAsync(Guid id)
        {
            using (var context = EventContext.Create())
            {
                var evt = await context.Events.Include(e => e.Properties).SingleOrDefaultAsync(e => e.Id == id);

                return Convert.ToEvent(evt);
            }
        }


        public async Task UpdateAsync(Guid id, Event evt)
        {
            using (var context = EventContext.Create())
            {
                var internalEvent = await context.Events.Include(e => e.Properties).SingleOrDefaultAsync(e => e.Id == id);

                if(internalEvent == null)
                {
                    throw new NullReferenceException($"Event {id} not found");
                }

                internalEvent.Category = evt.Category.ToString();
                internalEvent.Summary = evt.Summary;
                //internalEvent.TimestampUtc = evt.TimestampUtc; // Not sure we should allow this
                internalEvent.LastUpdatedUtc = DateTime.UtcNow;

                // We need to update any matching properties, or add new ones
                // @TODO: Jeez, how will this work for multiple properties with same category/name?
                // @HACK: I'm going to ignore this ^ for now!
				//		  - Ok, don't. It's causing an exception!
                //foreach(var p in evt.Properties)
                //{
                //    var internalProperty = internalEvent.Properties.Values(p.Category, p.Name).SingleOrDefault();

                //    if(internalProperty != null)
                //    {
                //        internalProperty.Value = p.Value;
                //    }
                //    else
                //    {
                //        // Add new
                //        internalEvent.Properties.Add(Convert.ToInternalProperty(p));
                //    }
                //}

                // .. and remove any properties no longer present.
                // @HACK: Ignoring this for now

                await context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<EventSummary>> SearchAsync(EventSearchCriteria criteria)
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
    }
}
