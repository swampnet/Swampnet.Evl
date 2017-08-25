using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory
{
    class EventContext : DbContext
    {
        public DbSet<InternalEvent> Events { get; set; }
    }
}
