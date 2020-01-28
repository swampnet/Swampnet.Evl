using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Swampnet.Evl.DAL.Entities;
using System;

namespace Swampnet.Evl.DAL
{
    public class EventsContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Source> Sources { get; set; }

        // This is bullshit
        private readonly string _connectionString;

        public EventsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("events");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Event>().HasMany(f => f.Properties).WithOne(f => f.Event).HasForeignKey(f => f.EventId);
            modelBuilder.Entity<Event>().HasOne(f => f.Category).WithMany(f => f.Events).HasForeignKey(f => f.CategoryId);
            modelBuilder.Entity<Event>().HasOne(f => f.Source).WithMany(f => f.Events).HasForeignKey(f => f.SourceId);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Source>().ToTable("Source");
            modelBuilder.Entity<EventProperty>().ToTable("EventProperty");

            base.OnModelCreating(modelBuilder);
        }
    }
}
