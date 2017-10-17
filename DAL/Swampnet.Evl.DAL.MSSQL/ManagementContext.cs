using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swampnet.Evl.DAL.MSSQL.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Swampnet.Evl.DAL.MSSQL
{
    class ManagementContext : DbContext
    {
        public ManagementContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalOrganisation> Organisations { get; set; }

        public static ManagementContext Create(string connectionString)
        {
            //Init(connectionString);

            var options = new DbContextOptionsBuilder<ManagementContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new ManagementContext(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InternalOrganisation>().ToTable("Organisation");
            modelBuilder.Entity<ApiKey>().ToTable("ApiKey");
        }


        #region HACK: Create tables and whatnot
        private static bool _init = false;

        private static void Init(string connectionString)
        {
            if (!_init)
            {
                var context = new ManagementContext(new DbContextOptionsBuilder<ManagementContext>()
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
