using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tt_apps_srs.Lib;

namespace tt_apps_srs.Models
{
    public class tt_apps_srs_db_context : DbContext
    {
        private readonly IAuditor _auditor;
        private readonly IClientProvider _client;

        public tt_apps_srs_db_context(DbContextOptions<tt_apps_srs_db_context> options) : base(options)
        {
            _auditor = this.GetService<IAuditor>();
            _client = this.GetService<IClientProvider>(); ;

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<ClientRetailer> ClientRetailers { get; set; }
        public DbSet<ClientRetailerProduct> ClientRetailerProducts {get;set;}
        public DbSet<Store> Stores { get; set; }
        public DbSet<ClientStore> ClientStores { get; set; }
        public DbSet<ClientStoreOrder> ClientStoreOrders {get;set;}
        public DbSet<ClientStoreOrderProduct> ClientStoreOrderProducts {get;set;}
        public DbSet<User> Users { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Client
            modelBuilder.Entity<Client>()
                        .HasIndex(i => i.UrlCode)
                        .IsUnique()
                        .HasName("IX_Client_UrlCode");

            modelBuilder.Entity<Client>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);
            #endregion

            #region Retailer

            modelBuilder.Entity<Retailer>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<Retailer>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_Retailer_Active");
            #endregion

            #region ClientRetailer
            

            modelBuilder.Entity<ClientRetailer>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<ClientRetailer>()
                        .HasIndex(i => new { i.Active, i.ClientId })
                        .HasName("IX_ClientRetailer_ClientActive");

            modelBuilder.Entity<ClientRetailer>()
                        .HasIndex(i => i.ClientId)
                        .HasName("IX_ClientRetailer_Client");

            modelBuilder.Entity<ClientRetailer>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_ClientRetailer_Active");

            modelBuilder.Entity<ClientRetailer>()
                        .HasOne(i => i.Client)
                        .WithMany(i => i.ClientRetailers)
                        .HasForeignKey(f => f.ClientId);

            modelBuilder.Entity<ClientRetailer>()
                        .HasOne(i => i.Retailer)
                        .WithMany(i => i.ClientRetailers)
                        .HasForeignKey(f => f.RetailerId);
                        
            #endregion

            #region ClientRetailerProduct

            modelBuilder.Entity<ClientRetailerProduct>()
                        .HasIndex( k => new { k.ClientRetailerId, k.UPC, k.SKU})
                        .IsUnique(true)
                        .HasName("IX_ClientRetailerId_UPC_SKU");
            
            modelBuilder.Entity<ClientRetailerProduct>()
                        .HasOne( h1 => h1.ClientRetailer)
                        .WithMany( m => m.ClientRetailerProducts)
                        .HasForeignKey( f => f.ClientRetailerId);

            modelBuilder.Entity<ClientRetailerProduct>()
                        .HasQueryFilter( q => q.ClientRetailer.ClientId == _client.ClientId);
                        

            #endregion

            #region Store

            modelBuilder.Entity<Store>()
                        .HasOne(r => r.Retailer)
                        .WithMany(r => r.Stores)
                        .HasForeignKey(f => f.RetailerId);

            modelBuilder.Entity<Store>()
                        .Property(p => p.Country)
                        .HasDefaultValue("US");

            modelBuilder.Entity<Store>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<Store>()
                        .HasIndex(i => new { i.Active, i.RetailerId })
                        .HasName("IX_Store_RetailerActive");

            modelBuilder.Entity<Store>()
                        .HasIndex(i => i.RetailerId)
                        .HasName("IX_Store_Retailer");

            modelBuilder.Entity<Store>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_Store_Active");
            #endregion

            #region ClientStore
            modelBuilder.Entity<ClientStore>()
                        .HasQueryFilter( q => q.ClientId == _client.ClientId);

            modelBuilder.Entity<ClientStore>()
                        .HasIndex(i => new { i.Active, i.ClientId })
                        .HasName("IX_ClientStore_ClientActive");

            modelBuilder.Entity<ClientStore>()
                        .HasIndex(i => i.ClientId)
                        .HasName("IX_ClientStore_Client");

            modelBuilder.Entity<ClientStore>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_ClientStore_Active");

            modelBuilder.Entity<ClientStore>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<ClientStore>()
                        .Property(p => p.CreatedAt)
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql<DateTime>("CURRENT_TIMESTAMP(6)");
            

            modelBuilder.Entity<ClientStore>()
                        .HasOne( h1 => h1.Store)
                        .WithMany( m => m.ClientStores)
                        .HasForeignKey( f => f.StoreId);

            
            modelBuilder.Entity<ClientStore>()
                        .HasOne( h1 => h1.Client)
                        .WithMany( m => m.ClientStores)
                        .HasForeignKey( f => f.ClientId);
                
            #endregion
/* 
            #region ClientProduct
            modelBuilder.Entity<ClientProduct>()
                        .HasQueryFilter( q => q.ClientId == _client.ClientId);

            modelBuilder.Entity<ClientProduct>()
                        .HasIndex(i => new { i.Active, i.ClientId })
                        .HasName("IX_ClientProduct_ClientActive");

            modelBuilder.Entity<ClientProduct>()
                        .HasIndex(i => i.ClientId)
                        .HasName("IX_ClientProduct_Client");

            modelBuilder.Entity<ClientProduct>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_ClientProduct_Active");

            modelBuilder.Entity<ClientProduct>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<ClientProduct>()
                        .HasOne(i => i.Client)
                        .WithMany(m => m.ClientProducts)
                        .HasForeignKey(f => f.ClientId)
                        .HasConstraintName( "FK_Client_ClientProduct_ClientId");
            #endregion
*/
/* 
            #region ClientProductStore
            modelBuilder.Entity<ClientProductStore>()
                        .HasKey(k => new { k.StoreId, k.ClientProductId })
                        .HasName("PK_ClientProductStore");

            #endregion

            #region ClientProductRetailer
            modelBuilder.Entity<ClientProductRetailer>()
                        .HasKey(k => new { k.RetailerId, k.ClientProductId })
                        .HasName("PK_ClientProductRetailer");

            #endregion
*/
            #region User

            modelBuilder.Entity<User>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<User>()
                        .HasIndex(i => new { i.Active, i.OpenIdIdentifier })
                        .HasName("IX_User_ActiveOpenIdIdentifer");
            #endregion

            #region ClientUser
            modelBuilder.Entity<ClientUser>()
                        .HasQueryFilter( q => q.ClientId == _client.ClientId);

            modelBuilder.Entity<ClientUser>()
                        .Property(p => p.Active)
                        .HasDefaultValue(true);

            modelBuilder.Entity<ClientUser>()
                        .HasIndex(i => new { i.Active, i.ClientId, i.UserId })
                        .HasName("IX_ClientUser_ActiveClientUser");

            modelBuilder.Entity<ClientUser>()
                        .HasIndex(i => i.Active)
                        .HasName("IX_ClientUser_Active");

            modelBuilder.Entity<ClientUser>()
                        .HasIndex(i => i.ClientId)
                        .HasName("IX_ClientUser_Client");

            modelBuilder.Entity<ClientUser>()
                        .HasOne(i => i.Client)
                        .WithMany(i => i.ClientUsers)
                        .HasForeignKey(f => f.ClientId);

            modelBuilder.Entity<ClientUser>()
                        .HasOne(i => i.User)
                        .WithMany(i => i.ClientUsers)
                        .HasForeignKey(f => f.UserId);
            #endregion

            #region ClientStoreOrder
                        
            modelBuilder.Entity<ClientStoreOrder>()
                        .HasIndex( i => i.Status)
                        .HasName("IX_ClientStoreOrder_Status");
            
            modelBuilder.Entity<ClientStoreOrder>()
                        .HasOne( h1 => h1.ClientStore)
                        .WithMany( m => m.Orders)
                        .HasForeignKey( f => f.ClientStoreId);
            #endregion 

            #region ClientStoreOrderProduct
            modelBuilder.Entity<ClientStoreOrderProduct>()
                        .HasKey( k => new { k.OrderId, k.ClientRetailerProductId})
                        .HasName( "PK_ClientStoreOrderProduct");

            modelBuilder.Entity<ClientStoreOrderProduct>()
                        .HasIndex( i => new { i.OrderId, i.Status})
                        .HasName( "IX_ClientStoreOrderProduct_Status");

            modelBuilder.Entity<ClientStoreOrderProduct>()
                        .HasOne( h1 => h1.Order)
                        .WithMany( m => m.Items)
                        .HasForeignKey( f => f.OrderId);
            
            modelBuilder.Entity<ClientStoreOrderProduct>()
                        .HasOne( h1 => h1.ClientRetailerProduct)
                        .WithMany( m => m.OrderItems)
                        .HasForeignKey( f => f.ClientRetailerProductId);


            #endregion
        }

        #region Auditing Mechanism

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChanges(auditEntries);
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
            return auditEntries.Where(_ => true || _.HasTemporaryProperties).ToList();
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

    public class Client 
    {

        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Required, MaxLength(64)]
        public string UrlCode { get; set; }

        [Column(TypeName="JSON")]
        public JsonObject<Dictionary<string, object>> Properties { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual ICollection<ClientStore> ClientStores { get; set; }
        public virtual ICollection<ClientRetailer> ClientRetailers { get; set; }

        public virtual ICollection<ClientUser> ClientUsers { get; set; }
    }

    public class Retailer
    {
        public Guid Id { get; set; }

        [Required, MaxLength(512)]
        public string Name { get; set; }


        [Required]
        public bool Active { get; set; }

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<ClientRetailer> ClientRetailers { get; set; }
    }

    public  class ClientRetailer
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Guid RetailerId { get; set; }

        [Column(TypeName="JSON")]
        public JsonObject<object> Properties { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual Client Client { get; set; }

        public virtual Retailer Retailer { get; set; }

        public virtual ICollection<ClientRetailerProduct> ClientRetailerProducts {get;set;}
    }

    public class ClientRetailerProduct
    {
        public int Id {get;set;}
        public int ClientRetailerId {get;set;}

        [MaxLength(50)]
        public string UPC {get;set;}
        [MaxLength(50)]
        public string SKU {get;set;}

        [MaxLength(255)]
        [Required]
        public string Name {get;set;}

        [Column(TypeName="DECIMAL(7,2)")]
        public decimal? Cost {get;set;}

        [Column(TypeName="JSON")]
        public JsonObject<IDictionary<string, object>> Properties {get;set;}

        public virtual ClientRetailer ClientRetailer {get;set;}
        public virtual ICollection<ClientStoreOrderProduct> OrderItems {get;set;}
    }

    public class Store
    {
        public Guid Id { get; set; }

        public Guid RetailerId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
        
        [Required, MaxLength(1024)]
        public string Addr_Ln_1 { get; set; }

        [MaxLength(512)]
        public string Addr_Ln_2 { get; set; }

        [Required, MaxLength(128)]
        public string City { get; set; }

        [Required, MaxLength(4)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        [Required, MaxLength(4)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Required]
        public bool Active { get; set; }
        public virtual Retailer Retailer { get; set; }

        public virtual IEnumerable<ClientStore> ClientStores { get; set; }

        public string Address
        {
            get
            {
                string retval = Addr_Ln_1;
                if (!String.IsNullOrEmpty(Addr_Ln_2))
                    retval += ", " + Addr_Ln_2;
                retval += String.Format(", {0}, {1}", City, State);
                if (!String.IsNullOrEmpty(Zip))
                    retval += " - " + Zip;

                return retval.Trim();
            }
        }

    }

    public class ClientStore
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Guid StoreId { get; set; }

        [Column(TypeName="JSON")]
        public JsonObject<Dictionary<string, object>> Properties { get; set; }

        [Required]
        public bool Active { get; set; }

        [Column(TypeName ="TIMESTAMP(6)")]
        public DateTime CreatedAt { get; set; }


        public virtual Store Store { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<ClientStoreOrder> Orders {get; set;}
    }

    public class ClientStoreOrder
    {
        public Guid Id { get; set; }
        public int ClientStoreId {get;set;}
        public decimal Total { get; set; }
        public string Notes { get; set; }
        [MaxLength(4)]
        public string Status { get; set; }
        public Guid CreatedBy { get; set; }
        [Column(TypeName="TIMESTAMP(6)")]
        public DateTime CreatedAt { get; set; }

        public Guid? VerifiedBy { get; set; }
        public DateTime? VerifiedAt { get; set; }


        public virtual ClientStore ClientStore { get; set; }

        public virtual ICollection<ClientStoreOrderProduct> Items { get; set; }

        [NotMapped]
        public virtual User CreatedByUser {get;set;}

        [NotMapped]
        public virtual User VerifiedByUser {get; set;}

    }

    public class ClientStoreOrderProduct
    {
        public Guid OrderId { get; set; }
        public int ClientRetailerProductId {get; set;}
        public uint Quantity { get; set; }

        [Column(TypeName="DECIMAL(7,2)")]
        public decimal UnitPrice { get; set; }

        [MaxLength(4)]
        public string Status { get; set; }

        public virtual ClientStoreOrder Order { get; set; }
        public virtual ClientRetailerProduct ClientRetailerProduct {get;set;}


    }

    

    /* 
    public class ClientProductStore : ClientProductEntity
    {

        public Guid StoreId { get; set; }
        public Guid ClientProductId { get; set; }

        public virtual Store Store { get; set; }

        public virtual ClientProduct ClientProduct { get; set; }
    }

    public class ClientProductRetailer : ClientProductEntity
    {
        public Guid RetailerId { get; set; }

        public Guid ClientProductId { get; set; }

        public virtual Retailer Retailer { get; set; }

        public virtual ClientProduct ClientProduct { get; set; }
    }
    */
    public class User
    {
        public Guid Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, MaxLength(512)]
        public string OpenIdIdentifier { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual ICollection<ClientUser> ClientUsers { get; set; }
    }

    public class ClientUser
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Guid UserId { get; set; }

        [Column(TypeName="JSON")]
        public JsonObject<Dictionary<string, object>> Properties { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual Client Client { get; set; }
        public virtual User User { get; set; }
    }

    #endregion

    #region Model Supports
    public class ClientProductEntity
    {

        [DataType(DataType.Currency)]
        public decimal? Cost_Per_Unit { get; set; }

        [Column(TypeName="JSON")]
        public JsonObject<Dictionary<string, object>> Properties { get; set; }
    }
    #endregion
}