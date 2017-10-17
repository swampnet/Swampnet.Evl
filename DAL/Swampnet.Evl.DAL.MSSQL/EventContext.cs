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
            //Init(connectionString);

            var options = new DbContextOptionsBuilder<EventContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new EventContext(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InternalProperty>().ToTable("Property");
            modelBuilder.Entity<InternalEvent>().ToTable("Event");
            modelBuilder.Entity<InternalEventProperties>().ToTable("EventProperties");
            modelBuilder.Entity<InternalEventTags>().ToTable("EventTags");
            modelBuilder.Entity<InternalTag>().ToTable("Tag");

            modelBuilder.Entity<InternalEventTags>().HasKey(x => new { x.EventId, x.InternalTagId });
            modelBuilder.Entity<InternalEventProperties>().HasKey(x => new { x.EventId, x.InternalPropertyId });
        }

        #region HACK: Create tables and whatnot
        private static bool _init = false;

        private static void Init(string connectionString)
        {
            if (!_init)
            {
                var context = new EventContext(new DbContextOptionsBuilder<EventContext>()
                    .UseSqlServer(connectionString)
                    .Options);
                var databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                databaseCreator.CreateTables();
                _init = true;
            }
        }
        #endregion
    }
}
