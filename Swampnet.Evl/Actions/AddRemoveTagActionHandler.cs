using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Actions
{
    class AddTagActionHandler : IActionHandler
    {
        public string Type => "add-tag";
        public string Name => "Add tag";
        public string Description => "@todo: Description for add-tag";

        public Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            if (actionDefinition.Properties != null && actionDefinition.Properties.Any())
            {
                if (evt.Tags == null)
                {
                    evt.Tags = new List<string>();
                }

                evt.Tags.Add(actionDefinition.Properties.StringValue("tag"));
            }

            return Task.CompletedTask;
        }

        public MetaDataCapture[] GetPropertyMetaData()
        {
            return new[]
            {
                new MetaDataCapture()
                {
                    Name = "tag",
                    Description = "Tag",
                    IsRequired = true,
                }
            };
        }
    }

    class RemoveTagActionHandler : IActionHandler
    {
        public string Type => "remove-tag";
        public string Name => "Remove tag";
        public string Description => "Remove a tag from the event";

        public Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            if (actionDefinition.Properties != null && actionDefinition.Properties.Any())
            {
                if (evt.Tags != null && evt.Tags.Any())
                {
                    var tag = actionDefinition.Properties.StringValue("tag");

                    evt.Tags.RemoveAll(t => t == tag);
                }
            }

            return Task.CompletedTask;
        }

        public MetaDataCapture[] GetPropertyMetaData()
        {
            return new[]
            {
                new MetaDataCapture()
                {
                    Name = "tag",
                    Description = "Tag",
                    IsRequired = true,
                }
            };
        }
    }

}
