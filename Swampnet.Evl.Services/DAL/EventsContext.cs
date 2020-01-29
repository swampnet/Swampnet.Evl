using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace Swampnet.Evl.Services.DAL
{
    class EventsContext : DbContext
    {
        public EventsContext(DbContextOptions<EventsContext> options)
            : base(options)
        {
        }

        public DbSet<EventEntity> Events { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SourceEntity> Sources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("events");
            modelBuilder.Entity<EventEntity>().ToTable("Event");
            modelBuilder.Entity<EventEntity>().HasMany(f => f.Properties).WithOne(f => f.Event).HasForeignKey(f => f.EventId);
            modelBuilder.Entity<EventEntity>().HasOne(f => f.Category).WithMany(f => f.Events).HasForeignKey(f => f.CategoryId);
            modelBuilder.Entity<EventEntity>().HasOne(f => f.Source).WithMany(f => f.Events).HasForeignKey(f => f.SourceId);

            modelBuilder.Entity<CategoryEntity>().ToTable("Category");
            modelBuilder.Entity<SourceEntity>().ToTable("Source");
            modelBuilder.Entity<EventPropertyEntity>().ToTable("EventProperty");

            base.OnModelCreating(modelBuilder);
        }
    }
}
