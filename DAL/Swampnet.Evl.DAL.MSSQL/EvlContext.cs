using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using Swampnet.Evl.Common.Entities;
using System.Collections.Generic;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.DAL.MSSQL
{
    class EvlContext : DbContext
    {
        public EvlContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<InternalEvent> Events { get; set; }
        public DbSet<InternalTag> Tags { get; set; }
        public DbSet<InternalOrganisation> Organisations { get; set; }
        public DbSet<InternalRule> Rules { get; set; }

        public static EvlContext Create(string connectionString)
        {
            //Seed.Init(connectionString);

            return new EvlContext(
                new DbContextOptionsBuilder<EvlContext>()
                    .UseSqlServer(connectionString)
                    .Options);
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

            modelBuilder.Entity<InternalOrganisation>().ToTable("Organisation");
            modelBuilder.Entity<InternalOrganisation>().Property(f => f.Description).IsRequired();
            modelBuilder.Entity<InternalOrganisation>().Property(f => f.Name).IsRequired();

            modelBuilder.Entity<ApiKey>().ToTable("ApiKey");
            modelBuilder.Entity<ApiKey>().Property(f => f.OrganisationId).IsRequired();

            modelBuilder.Entity<InternalRule>().ToTable("Rule");
            modelBuilder.Entity<InternalRule>().Property(f => f.ActionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.ExpressionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.Name).IsRequired();
            modelBuilder.Entity<InternalRule>().Property(f => f.IsActive).IsRequired();
        }
    }
}
