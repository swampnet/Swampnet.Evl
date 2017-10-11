using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    /// <summary>
    /// Entity used internally to persist event data
    /// </summary>
    class InternalEvent
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdatedUtc { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
		public string Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

        /// <summary>
        /// Any additional data associated with the event
        /// </summary>
		public List<InternalProperty> Properties { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<InternalEventTags> InternalEventTags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SourceVersion { get; set; }


        internal void AddTags(EventContext context, IEnumerable<string> tags)
        {
            List<InternalEventTags> links = null;

            if (tags != null && tags.Any())
            {
                links = new List<InternalEventTags>();
                foreach (var tag in tags)
                {
                    var link = new InternalEventTags();
                    link.Event = this;

                    var t = context.Tags.FirstOrDefault(x => x.Name == tag); // .First() - it *is* possible to have multiple tags with same name (due to syncronisation, or lack of lol!)
                    if (t == null)
                    {
                        t = new InternalTag()
                        {
                            Name = tag
                        };
                        context.Tags.Add(t);
                    }
                    link.Tag = t;
                    links.Add(link);
                }
            }

            if(InternalEventTags == null)
            {
                InternalEventTags = links;
            }
            else
            {
                InternalEventTags.AddRange(links);
            }
        }
    }
}
