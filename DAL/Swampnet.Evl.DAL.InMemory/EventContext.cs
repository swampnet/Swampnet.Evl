﻿using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory
{
    class EventContext : DbContext
    {
        public EventContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalEvent> Events { get; set; }


        public static EventContext Create()
        {
            var options = new DbContextOptionsBuilder<EventContext>()
                .UseInMemoryDatabase("evl-database")
                .Options;

            return new EventContext(options);
        }
    }
}