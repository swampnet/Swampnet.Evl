using Microsoft.EntityFrameworkCore;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using System.Linq;
using Swampnet.Evl.DAL.MSSQL.Entities;
using Microsoft.Extensions.Configuration;
using Serilog;
using Swampnet.Evl.Client;
using Swampnet.Evl.Internal;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class ManagementDataAccess : IManagementDataAccess
    {
        private readonly IConfiguration _cfg;

        public ManagementDataAccess(IConfiguration cfg)
        {
            _cfg = cfg;
        }


        public async Task<Organisation> LoadOrganisationByApiKeyAsync(Guid apiKey)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations
                    .Include(o => o.ApiKeys)
                    .Include(o => o.InternalOrganisationProperties)
                        .ThenInclude(p => p.Property)
                    .FirstOrDefaultAsync(o => 
                        o.ApiKeys.Any(k => 
                            k.Id == apiKey 
                            && (k.RevokedOnUtc == null || k.RevokedOnUtc > DateTime.UtcNow)));

                return Convert.ToOrganisation(org);
            }
        }


        public async Task<Organisation> LoadOrganisationAsync(Guid id)
        {
            using (var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                var org = await context.Organisations
                    .Include(o => o.InternalOrganisationProperties)
                    .ThenInclude(p => p.Property)
                    .FirstOrDefaultAsync(o => o.Id == id);

                return Convert.ToOrganisation(org);
            }
        }


        public async Task<Organisation> UpdateOrganisationAsync(Guid id, Organisation source)
        {
            using(var context = EvlContext.Create(_cfg.GetConnectionString(EvlContext.CONNECTION_NAME)))
            {
                // Load existing entity
                var org = await context.Organisations
                    .Include(o => o.InternalOrganisationProperties)
                    .ThenInclude(p => p.Property)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if(org == null)
                {
                    throw new NullReferenceException($"Failed to load organisation {id}");
                }

                var updates = new List<Property>();

                // Update stuff that may have changed
                if (!string.IsNullOrEmpty(source.Name) && source.Name != org.Name)
                {
                    updates.Add(new Property("Update", "Modify",  $"Name changed from '{org.Name}' to '{source.Name}'"));
                    org.Name = source.Name;
                }

                if (!string.IsNullOrEmpty(source.Description) && source.Description != org.Description)
                {
                    updates.Add(new Property("Update", "Modify", $"Description changed from '{org.Description}' to '{source.Description}'"));
                    org.Description = source.Description;
                }

                // Update properties
                var properties = org.GetProperties();

                foreach (var prp in source.Properties)
                {
                    var existingProperty = properties.SingleOrDefault(p => p.Category == prp.Category && p.Name == prp.Name);
                    // Update existing
                    if(existingProperty != null)
                    {
                        // Remove
                        if (string.IsNullOrEmpty(prp.Value))
                        {
                            // Remove the property
                            org.RemoveProperty(existingProperty);
                            context.Properties.Remove(existingProperty);
                            updates.Add(new Property("Update", "Delete", $"Delete property {existingProperty}"));
                        }
                        // Update
                        else if(existingProperty.Value != prp.Value)
                        {
                            updates.Add(new Property("Update", "Modify", $"Property '{existingProperty.Name}' value changed from '{existingProperty.Value}' to '{prp.Value}'"));
                            existingProperty.Value = prp.Value;
                        }
                    }
                    // Add new
                    else if(!string.IsNullOrEmpty(prp.Value))
                    {
                        updates.Add(new Property("Update", "Create", $"Create property {prp}"));
                        org.InternalOrganisationProperties.Add(new InternalOrganisationProperties()
                        {
                            Organisation = org,
                            Property = new InternalProperty()
                            {
                                Category = prp.Category,
                                Name = prp.Name,
                                Value = prp.Value
                            }
                        });
                    }
                }

                await context.SaveChangesAsync();

                Log.Logger
                    .WithProperties(updates)
                    .WithProperty(new Property("__override__", "organisation-id", org.Id.ToString()))
                    .Information("Configuration updated");

                return Convert.ToOrganisation(org);
            }
        }
    }
}
