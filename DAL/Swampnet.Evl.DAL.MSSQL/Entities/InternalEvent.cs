using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    /// <summary>
    /// Entity used internally to persist event data
    /// </summary>
    class InternalEvent
    {
        public Guid Id { get; set; }

        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
		public string Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

		public List<InternalEventProperties> InternalEventProperties { get; set; }

        public List<InternalEventTags> InternalEventTags { get; set; }

        public List<InternalTrigger> Triggers { get; set; }

        public string Source { get; set; }

        public string SourceVersion { get; set; }

        public InternalOrganisation Organisation { get; set; }

        internal IEnumerable<string> GetTagNames()
        {
            var tags = Enumerable.Empty<string>();

            if(InternalEventTags != null && InternalEventTags.Any())
            {
                tags = InternalEventTags.Select(t => t.Tag.Name);
            }

            return tags;
        }

        internal void AddTriggers(IEnumerable<Trigger> triggers)
        {
        }

        internal void AddTrigger(Trigger trigger)
        {
            if(Triggers == null)
            {
                Triggers = new List<InternalTrigger>();
            }

            Triggers.Add(Convert.ToTrigger(trigger));
        }


        internal void AddTag(EvlContext context, string tag)
        {
            if(!string.IsNullOrEmpty(tag) && !GetTagNames().Contains(tag))
            {
                var link = new InternalEventTags();
                link.Event = this;

                var t = context.Tags.FirstOrDefault(x => x.Name == tag); // .First() - it *is* possible to have multiple tags with same name (due to syncronisation, or lack of lol!)
                if (t == null)
                {
                    t = new InternalTag()
                    {
                        Name = tag.Truncate(100)
                    };
                    context.Tags.Add(t);
                }
                link.Tag = t;

                if (InternalEventTags == null)
                {
                    InternalEventTags = new List<Entities.InternalEventTags>();
                }
                InternalEventTags.Add(link);
            }
        }

        internal void AddTags(EvlContext context, IEnumerable<string> tags)
        {
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    AddTag(context, tag);
                }
            }
        }


        internal void AddProperty(IProperty property)
        {
            // Don't add dups
            if (GetProperties().Any(x => x.Category == property.Category && x.Name == property.Name && x.Value == property.Value))
            {
                return;
            }
            
            if(InternalEventProperties == null)
            {
                InternalEventProperties = new List<Entities.InternalEventProperties>();
            }

            InternalEventProperties.Add(new InternalEventProperties(this, Convert.ToInternalProperty(property)));
        }


        internal void AddProperties(IEnumerable<IProperty> properties)
        {
            if (properties != null && properties.Any())
            {
                foreach (var prp in properties)
                {
                    AddProperty(prp);
                }
            }
        }

        internal IEnumerable<InternalProperty> GetProperties()
        {
            return InternalEventProperties == null
                ? Enumerable.Empty<InternalProperty>()
                : InternalEventProperties.Select(p => p.Property);
        }
    }
}
