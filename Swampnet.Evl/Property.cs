using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public interface IProperty
    {
        string Category { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }

    public class Property : IProperty
    {
        public Property()
        {
        }

        public Property(string name, object value, string category = null)
            : this()
        {
            Name = name;
            Value = value?.ToString();
            Category = category;
        }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
