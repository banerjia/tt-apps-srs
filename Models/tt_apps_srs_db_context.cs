using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace tt_apps_srs.Models
{
    public class tt_apps_srs_db_context: DbContext
    {
        private readonly IAuditor _auditor;
        public tt_apps_srs_db_context(DbContextOptions<tt_apps_srs_db_context> options): base(options)
        {
            _auditor = this.GetService<IAuditor>();
        }

        public DbSet<Tenant> Tenants {get; set;}
        public DbSet<TenantProperty> TenantProperties {get;set;}

        public DbSet<Store> Stores {get;set;}

        public DbSet<TenantStore> TenantStores { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Tenant>()
                .HasIndex(i => i.UrlCode)
                .IsUnique()
                .HasName("IX_Tenant_UrlCode");
            modelBuilder.Entity<Tenant>()
                .HasIndex(i => i.Name)
                .IsUnique()
                .HasName("IX_Tenant_Name");
        }

        #region Auditing Mechanism

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChanges( auditEntries);
            return result;
        }
        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.Relational().TableName;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => true ||  _.HasTemporaryProperties).ToList();
        }
        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                _auditor.Create(auditEntry.ToAudit());
            }

            return Task.CompletedTask;
        }

        #endregion
    }



    #region Model Definitions

    public class Tenant 
    {
        public Guid Id { get; set; }

        [Required, MaxLength(128)]
        public string Name {get;set;}
        [Required, MaxLength(64)]
        public string UrlCode { get; set; }
    }

    public class TenantProperty
    {
        public int Id { get; set; }

        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
    }

    public class Store
    {
        public Guid Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(1024)]
        public string add_ln_1 { get; set; }
        [MaxLength(512)]
        public string addr_ln_2 {get;set;}
        [Required, MaxLength(128)]
        public string City { get; set; }
        [Required, MaxLength(4)]
        public string State { get; set; }
    }

    public class TenantStore
    {
        public int Id { get; set; }

        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }

        [ForeignKey("Store")]
        public Guid StoreId { get; set; }
    }
#endregion
}