using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class RuleContext : DbContext
    {
        public RuleContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalRule> Rules { get; set; }

        public static RuleContext Create(string connectionString)
        {
            //Init(connectionString);

            var options = new DbContextOptionsBuilder<RuleContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new RuleContext(options);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InternalRule>().ToTable("Rule");
            modelBuilder.Entity<InternalRule>().Property(f => f.ActionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.ExpressionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.Name).IsRequired();
            modelBuilder.Entity<InternalRule>().Property(f => f.IsActive).IsRequired();
        }

        #region HACK: Create tables and whatnot
        private static bool _init = false;

        private static void Init(string connectionString)
        {
            if (!_init)
            {
                var context = new RuleContext(new DbContextOptionsBuilder<RuleContext>()
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
