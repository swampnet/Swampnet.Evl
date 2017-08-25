﻿using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

                internalEvent.Category = evt.Category;
                internalEvent.Summary = evt.Summary;
                internalEvent.TimestampUtc = evt.TimestampUtc; // Not sure we should allow this

                // We need to update any matching properties, or add new ones
                // @TODO: Jeez, how will this work for multiple properties with same category/name?
                // @HACK: I'm going to ignore this ^ for now!
                foreach(var p in evt.Properties)
                {
                    var internalProperty = internalEvent.Properties.Values(p.Category, p.Name).SingleOrDefault();

                    if(internalProperty != null)
                    {
                        internalProperty.Value = p.Value;
                    }
                    else
                    {
                        // Add new
                        internalEvent.Properties.Add(Convert.ToInternalProperty(p));
                    }
                }

                // .. and remove any properties no longer present.
                // @HACK: Ignoreing this for now

                await context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<Event>> SearchAsync()
        {
            throw new NotImplementedException();
        }
    }
}