using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Actions
{
    class ChangeCategoryActionHandler : IActionHandler
    {
        public string Type => "change-category";
        public string Name => "Change category";
        public string Description => "Change event category";


        public Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            var cat = actionDefinition.Properties.StringValue("category");
            if(!string.IsNullOrEmpty(cat))
            {
                evt.Category = Enum.Parse<EventCategory>(cat, true);
            }

            return Task.CompletedTask;
        }

        public MetaDataCapture[] GetPropertyMetaData()
        {
            return new[]
            {
                new MetaDataCapture()
                {
                    Name = "Category",
                    Description = "Category",
                    IsRequired = true,
                    DataType = "select",
                    Options = new[]
                    {
                        new Option("Information", "Information"),
                        new Option("Error", "Error"),
                        new Option("Debug", "Debug")
                    }
                }
            };
        }

    }
}
