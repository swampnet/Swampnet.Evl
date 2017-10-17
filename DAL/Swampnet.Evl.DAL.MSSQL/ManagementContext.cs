using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swampnet.Evl.DAL.MSSQL.Entities;

namespace Swampnet.Evl.DAL.MSSQL
{
    class ManagementContext : DbContext
    {
        public ManagementContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalOrganisation> Organisations { get; set; }

        public static ManagementContext Create()
        {
            var options = new DbContextOptionsBuilder<ManagementContext>()
                //.UseInMemoryDatabase("evl-database")
                .Options;

            return new ManagementContext(options);
        }
    }
}
