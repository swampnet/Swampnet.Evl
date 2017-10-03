﻿using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    /// <summary>
    /// Entity used internally to persist a generic Property
    /// </summary>
    internal class InternalProperty : IProperty
    {
        public long Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Category)
                ? $"{Id} - {Name} = {Value}"
                : $"{Id} - [{Category}] {Name} = {Value}";
        }


        public InternalEvent Event { get; set; }
    }
}
