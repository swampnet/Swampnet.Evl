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
            var options = new DbContextOptionsBuilder<ManagementContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new ManagementContext(options);

            //using Microsoft.EntityFrameworkCore.Infrastructure;
            //RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
            //databaseCreator.CreateTables();
        }
    }
}
