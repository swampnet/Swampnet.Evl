using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL
{
    class EventContext : DbContext
    {
        public EventContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalEvent> Events { get; set; }
        public DbSet<InternalTag> Tags { get; set; }

        public static EventContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<EventContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new EventContext(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InternalEventTags>().HasKey(x => new { x.EventId, x.InternalTagId });
        }
    }
}
