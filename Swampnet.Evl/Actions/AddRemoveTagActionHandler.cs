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

        public Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
        {
            if (actionDefinition.Properties != null && actionDefinition.Properties.Any())
            {
                if (evt.Tags != null && evt.Tags.Any())
                {
                    evt.Tags.Remove(actionDefinition.Properties.StringValue("tag"));
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
