using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.DAL.InMemory.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory
{
    class RuleContext : DbContext
    {
        public RuleContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<InternalRule> Rules { get; set; }

        public static RuleContext Create()
        {
            var options = new DbContextOptionsBuilder<RuleContext>()
                .UseInMemoryDatabase("evl-database")
                .Options;

            return new RuleContext(options);
        }
    }
}
