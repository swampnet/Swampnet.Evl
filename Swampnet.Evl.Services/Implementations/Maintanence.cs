using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class Maintanence : IMaintanence
    {
        private readonly EventsContext _context;

        public Maintanence(EventsContext context)
        {
            _context = context;
        }


        public async Task RunAsync()
        {
            // Clean up duplicate source
            //var x = await _context.Sources
            //    .GroupBy(s => s.Name)
            //    .Where(s => s.Count() > 1)
            //    .Select(g => new
            //    {
            //        Id = g.Key,
            //        Count = g.Count()
            //    })
            //    .ToArrayAsync();

            var sources = await _context.Sources.ToArrayAsync();

            foreach (var target in sources.GroupBy(s => s.Name).Where(s => s.Count() > 1))
            {
                var consolidateOn = target.First();
                var remove = target.Where(i => i != consolidateOn);
                var remove_id = remove.Select(i => i.Id);
                var events = await _context.Events.Where(e => remove_id.Contains(e.SourceId)).ToArrayAsync();

                foreach (var e in events)
                {
                    e.SourceId = consolidateOn.Id;
                }

                _context.Sources.RemoveRange(remove);
            }

            await _context.SaveChangesAsync();
        }
    }
}
