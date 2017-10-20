using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class EventDetails : Event
    {
        public Guid Id { get; set; }
        public Trigger[] Triggers { get; set; }
    }


    public class Trigger
    {
        public DateTime TimestampUtc { get; set; }
        public string RuleName { get; set; }
        public Action[] Actions { get; set; }
    }


    public class Action
    {
        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }
        public Property[] Properties { get; set; }
    }
}
