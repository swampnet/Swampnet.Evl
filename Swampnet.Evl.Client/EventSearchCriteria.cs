﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Client
{
    public class EventSearchCriteria
    {
        public Guid? Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EventCategory? Category { get; set; }

        public string Summary { get; set; }

        public DateTime? TimestampUtc { get; set; }

        public string Source { get; set; }

        public string SourceVersion { get; set; }

		public string Tags { get; set; }

		#region Advanced
		public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        #endregion

    }
}
