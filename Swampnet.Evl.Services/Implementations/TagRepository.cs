using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations
{
    class TagRepository : ITagRepository
    {
        private readonly EventsContext _context;
        private static readonly Dictionary<string, TagEntity> _cache = new Dictionary<string, TagEntity>();

        public TagRepository(EventsContext context)
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

            var entity = await _context.Tags.Where(t => t.Name == lookup).OrderBy(t => t.Id).FirstOrDefaultAsync();
            if(entity != null)
            {
                lock (_cache)
                {
                    if (!_cache.ContainsKey(lookup))
                    {
                        _cache.Add(lookup, entity);
                    }
                }
                return entity;
            }

            // Create new tag. Note that we might be creating a dup. here so don't add it to the cache.
            entity = new TagEntity()
            {
                Name = name
            };

            _context.Tags.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
