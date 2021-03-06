﻿using Swampnet.Evl.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Organisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public Guid[] ApiKeys { get; set; }
        public Property[] Properties { get; set; }

        public string GetConfigurationValue(string category, string name)
        {
            return Properties == null
                ? ""
                : Properties.Where(p => p.Category.EqualsNoCase(category)).StringValue(name, "");
        }
    }
}
