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
            modelBuilder.Entity<InternalProperty>().Property(f => f.Name).IsRequired().HasMaxLength(225);
            modelBuilder.Entity<InternalProperty>().Property(f => f.Value).IsRequired();
            modelBuilder.Entity<InternalProperty>().Property(f => f.Category).HasMaxLength(225);
            modelBuilder.Entity<InternalProperty>().HasIndex(f => new { f.Category, f.Name });
            modelBuilder.Entity<InternalProperty>().HasIndex(f => f.Name);

            modelBuilder.Entity<InternalEvent>().ToTable("Event");
            modelBuilder.Entity<InternalEvent>().Property(f => f.Category).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<InternalEvent>().Property(f => f.Source).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<InternalEvent>().Property(f => f.Summary).IsRequired();

            modelBuilder.Entity<InternalEventProperties>().ToTable("EventProperties");
            modelBuilder.Entity<InternalEventProperties>().HasKey(x => new { x.EventId, x.InternalPropertyId });

            modelBuilder.Entity<InternalEventTags>().ToTable("EventTags");
            modelBuilder.Entity<InternalEventTags>().HasKey(x => new { x.EventId, x.InternalTagId });

            modelBuilder.Entity<InternalTag>().ToTable("Tag");
            modelBuilder.Entity<InternalTag>().Property(f => f.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InternalTag>().HasIndex(x => x.Name);
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
