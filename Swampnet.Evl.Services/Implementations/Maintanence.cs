using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    /// <summary>
    /// Maintanence jobs
    /// 
    /// - Fix up duplicate source
    /// - truncate data
    ///  - based on category and age
    /// </summary>
    class Maintanence : IMaintanence
    {
        private readonly EventsContext _context;

        public Maintanence(EventsContext context)
        {
            _context = context;
        }


        public async Task RunAsync()
        {
            await CleanupDuplicateSourceAsync();
            await CleanupDuplicateTagsAsync();
        }


        private async Task CleanupDuplicateTagsAsync()
        {
            var tags = await _context.Tags.ToArrayAsync();
            foreach (var target in tags.GroupBy(t => t.Name).Where(t => t.Count() > 1))
            {
                var consolidateOn = target.First();
                var remove = target.Where(i => i != consolidateOn);
                var remove_id = remove.Select(i => i.Id);
                var eventTags = await _context.EventTags.Where(e => remove_id.Contains(e.TagId)).ToArrayAsync();
                foreach (var e in eventTags)
                {
                    e.TagId = consolidateOn.Id;
                }

                _context.Tags.RemoveRange(remove);
            }

            await _context.SaveChangesAsync();
        }


        private async Task CleanupDuplicateSourceAsync()
        {
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
