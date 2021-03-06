﻿using System;
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
        public InternalEvent()
        {
            Properties = new List<InternalEventProperty>();
        }

        public Guid Id { get; set; }

        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        public DateTime ModifiedOnUtc { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
		public string Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

		//public List<InternalEventProperties> InternalEventProperties { get; set; }
        public List<InternalEventProperty> Properties { get; set; }

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
			if(triggers != null)
			{
				foreach(var trigger in triggers)
				{
					AddTrigger(trigger);
				}
			}
        }

        internal void AddTrigger(Trigger trigger)
        {
            if(Triggers == null)
            {
                Triggers = new List<InternalTrigger>();
            }

            Triggers.Add(Convert.ToTrigger(trigger));
        }


        internal void AddTag(EvlContext context, string tag, Guid orgid)
        {
            if(!string.IsNullOrEmpty(tag) && !GetTagNames().Any(t => t.EqualsNoCase(tag)))
            {
                var link = new InternalEventTags();
                link.Event = this;

                var t = context.Tags.FirstOrDefault(x => x.OrganisationId == orgid && x.Name == tag); // .First() - it *is* possible to have multiple tags with same name (due to syncronisation, or lack of lol!)
                if (t == null)
                {
                    t = new InternalTag()
                    {
                        Name = tag.Truncate(100),
                        OrganisationId = orgid
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


        internal void AddTags(EvlContext context, IEnumerable<string> tags, Guid orgid)
        {
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    AddTag(context, tag, orgid);
                }
            }
        }


        internal void AddProperty(IProperty property)
        {
            // Don't add dups
            if (Properties.Any(x => x.Category == property.Category && x.Name == property.Name && x.Value == property.Value))
            {
                return;
            }
            
            Properties.Add(new InternalEventProperty()
            {
                Event = this,
                Category = property.Category.Truncate(128),
                Name = property.Name.Truncate(128),
                Value = property.Value == null ? "[null]" : property.Value.Truncate(8000, true)
            });
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

    }
}
