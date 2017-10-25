﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    /// <summary>
    /// Entity used internally to persist a Rule
    /// </summary>
    internal class InternalRule
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        // Serialised expression data
        public string ExpressionData { get; set; }

        // Serialised action data
        public string ActionData { get; set; }
    }
}
