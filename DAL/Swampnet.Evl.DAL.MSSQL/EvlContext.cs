using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System.Linq;

namespace Swampnet.Evl.DAL.MSSQL
{
    class EvlContext : DbContext
    {
		public const string CONNECTION_NAME = "swampnet-evl";
		public const string SCHEMA = "evl";

		public EvlContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<InternalEvent> Events { get; set; }
        public DbSet<InternalTag> Tags { get; set; }
        public DbSet<InternalProfile> Profiles { get; set; }
        public DbSet<InternalOrganisation> Organisations { get; set; }
        public DbSet<InternalRule> Rules { get; set; }
        public DbSet<InternalRole> Roles { get; set; }
		public DbSet<InternalPermission> Permissions { get; set; }


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

            modelBuilder.Entity<InternalProperty>().ToTable("Property", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalProperty>().Property(f => f.Name).IsRequired().HasMaxLength(225);
            modelBuilder.Entity<InternalProperty>().Property(f => f.Value).IsRequired();
            modelBuilder.Entity<InternalProperty>().Property(f => f.Category).HasMaxLength(225);
            modelBuilder.Entity<InternalProperty>().HasIndex(f => new { f.Category, f.Name });
            modelBuilder.Entity<InternalProperty>().HasIndex(f => f.Name);

            modelBuilder.Entity<InternalEvent>().ToTable("Event", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalEvent>().Property(f => f.Category).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<InternalEvent>().Property(f => f.Source).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<InternalEvent>().Property(f => f.Summary).IsRequired();

            modelBuilder.Entity<InternalEventProperties>().ToTable("EventProperties", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalEventProperties>().Property(f => f.InternalPropertyId).HasColumnName("PropertyId");
            modelBuilder.Entity<InternalEventProperties>().HasKey(x => new { x.EventId, x.InternalPropertyId });

            modelBuilder.Entity<InternalEventTags>().ToTable("EventTags", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalEventTags>().Property(f => f.InternalTagId).HasColumnName("TagId");
            modelBuilder.Entity<InternalEventTags>().HasKey(x => new { x.EventId, x.InternalTagId });

            modelBuilder.Entity<InternalActionProperties>().ToTable("ActionProperties", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalActionProperties>().Property(f => f.InternalPropertyId).HasColumnName("PropertyId");
            modelBuilder.Entity<InternalActionProperties>().HasKey(x => new { x.ActionId, x.InternalPropertyId });

            modelBuilder.Entity<InternalTag>().ToTable("Tag", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalTag>().Property(f => f.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InternalTag>().HasIndex(x => x.Name);
            modelBuilder.Entity<InternalTag>().HasMany(f => f.InternalEventTags).WithOne(f => f.Tag).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InternalOrganisation>().ToTable("Organisation", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalOrganisation>().Property(f => f.Description).IsRequired();
            modelBuilder.Entity<InternalOrganisation>().Property(f => f.Name).IsRequired();

            modelBuilder.Entity<InternalProfile>().ToTable("Profile", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalProfile>().Property(f => f.Key).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<InternalProfile>().Property(f => f.InternalOrganisationId).HasColumnName("OrganisationId");
            modelBuilder.Entity<InternalProfile>().HasKey(f => f.Id);
            modelBuilder.Entity<InternalProfile>().HasIndex(f => f.Key);

            modelBuilder.Entity<InternalRole>().ToTable("Role", EvlContext.SCHEMA);

            modelBuilder.Entity<InternalProfileRole>().ToTable("ProfileRoles", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalProfileRole>().HasKey(x => new { x.ProfileId, x.RoleId });

			modelBuilder.Entity<InternalPermission>().ToTable("Permission", EvlContext.SCHEMA);
			modelBuilder.Entity<InternalRolePermission>().ToTable("RolePermissions", EvlContext.SCHEMA);
			modelBuilder.Entity<InternalRolePermission>().HasKey(x => new { x.PermissionId, x.RoleId });

			modelBuilder.Entity<ApiKey>().ToTable("ApiKey", EvlContext.SCHEMA);
            modelBuilder.Entity<ApiKey>().Property(f => f.OrganisationId).IsRequired();

            modelBuilder.Entity<InternalRule>().ToTable("Rule", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalRule>().Property(f => f.ActionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.ExpressionData).IsRequired().HasColumnType("xml");
            modelBuilder.Entity<InternalRule>().Property(f => f.Name).IsRequired();
            modelBuilder.Entity<InternalRule>().Property(f => f.IsActive).IsRequired();

            modelBuilder.Entity<InternalTrigger>().ToTable("Trigger", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalTrigger>().Property(f => f.RuleName).IsRequired();
            modelBuilder.Entity<InternalTrigger>().Property(f => f.TimestampUtc).IsRequired();

            modelBuilder.Entity<InternalAction>().ToTable("Action", EvlContext.SCHEMA);
            modelBuilder.Entity<InternalAction>().Property(f => f.Type).IsRequired();
            modelBuilder.Entity<InternalAction>().Property(f => f.TimestampUtc).IsRequired();
        }
    }
}
