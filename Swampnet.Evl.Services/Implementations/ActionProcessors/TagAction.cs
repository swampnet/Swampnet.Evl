using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations.ActionProcessors
{
    class AddTagAction : IActionProcessor
    {
        private readonly ITags _tags;

        public AddTagAction(ITags tags)
        {
            _tags = tags;
        }

        public string Name => "add-tag";

        public async Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition)
        {
            var tagName = definition.Properties.StringValue("tag");
            
            if(!evt.EventTags.Any(et => et.Tag.Name.EqualsNoCase(tagName)))
            {
                var tag = await _tags.ResolveAsync(tagName);

                evt.EventTags.Add(new EventTagsEntity() { 
                    Tag = await context.Tags.SingleAsync(t => t.Id == tag.Id)
                });
            }
        }
    }

    class RemoveTagAction : IActionProcessor
    {
        public string Name => "remove-tag";

        public Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition)
        {
            var tagName = definition.Properties.StringValue("tag");

            foreach(var t in evt.EventTags.Where(et => et.Tag.Name.EqualsNoCase(tagName)).ToArray())
            {
                evt.EventTags.Remove(t);
                context.EventTags.Remove(t);
            }

            return Task.CompletedTask;
        }
    }
}
