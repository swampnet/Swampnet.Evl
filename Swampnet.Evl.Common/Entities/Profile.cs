using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Profile
    {
        public long Id { get; set; }

        public List<Group> Groups { get; set; }

        public Name Name { get; set; }

        public Organisation Organisation { get; set; }
    }


    public class Name
    {
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string KnownAs { get; set; }
    }
}
