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
            var options = new DbContextOptionsBuilder<RuleContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new RuleContext(options);
        }
    }
}
