using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Swampnet.Evl.DAL.InMemory
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
                .UseInMemoryDatabase("evl-database")
                .Options;

            return new ManagementContext(options);
        }
    }
}
