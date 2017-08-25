﻿using Swampnet.Evl.Common;
using Swampnet.Evl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Interfaces
{
    public interface IEventDataAccess
    {
        long Create(Application app, Event evt);
        Event Read(long id);
        void Update(long id, Event evt);
        IEnumerable<Event> Search(/* Criteria */);
    }
}
