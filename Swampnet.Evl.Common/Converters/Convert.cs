using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common
{
    public static class Convert
    {
        public static EventDetails ToEventDetails(Event source)
        {
            return source == null
                ? null
                : new EventDetails()
                {
                    Category = source.Category,
                    LastUpdatedUtc = source.LastUpdatedUtc,
                    Source = source.Source,
                    Properties = source.Properties,
                    SourceVersion = source.SourceVersion,
                    Summary = source.Summary,
                    Tags = source.Tags,
                    TimestampUtc = source.TimestampUtc
                };
        }
    }
}
