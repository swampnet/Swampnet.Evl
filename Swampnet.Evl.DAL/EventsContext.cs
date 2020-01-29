﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Swampnet.Evl.DAL.Entities;
using System;

namespace Swampnet.Evl.DAL
{
    public class EventsContext : DbContext
    {
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SourceEntity> Sources { get; set; }

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
