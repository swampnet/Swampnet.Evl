using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations.ActionProcessors
{
    class SetCategoryAction : IActionProcessor
    {
        public string Name => "set-category";

        public async Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition)
        {
            var cat = definition.Properties.StringValue("category");
            if (!string.IsNullOrEmpty(cat) && !evt.Category.Name.EqualsNoCase(cat))
            {
                evt.Category = await context.Categories.SingleOrDefaultAsync(c => c.Name == cat);
                evt.CategoryId = evt.Category.Id;
            }
        }
    }
}
