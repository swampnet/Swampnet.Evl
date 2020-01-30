using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class TagService : ITags
    {
        private readonly EventsContext _context;
        private static readonly Dictionary<string, TagEntity> _cache = new Dictionary<string, TagEntity>();

        public TagService(EventsContext context)
        {
            _context = context;
        }

        public async Task<TagEntity> ResolveAsync(string name)
        {
            var lookup = name.ToLower().Trim();

            if (_cache.ContainsKey(lookup))
            {
                return _cache[lookup];
            }

            var tag = await _context.Tags.Where(t => t.Name == lookup).OrderBy(t => t.Id).FirstOrDefaultAsync();
            if(tag != null)
            {
                _cache.Add(lookup, tag);
                return tag;
            }

            // Create new tag. Note that we might be creating a dup. here so don't add it to the cache.
            tag = new TagEntity()
            {
                Name = name
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return tag;
        }
    }
}
