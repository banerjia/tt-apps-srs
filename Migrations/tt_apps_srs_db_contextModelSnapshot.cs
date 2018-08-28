﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tt_apps_srs.Models;

namespace tt_apps_srs.Migrations
{
    [DbContext(typeof(tt_apps_srs_db_context))]
    partial class tt_apps_srs_db_contextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("tt_apps_srs.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Properties");

                    b.Property<string>("UrlCode")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("UrlCode")
                        .IsUnique()
                        .HasName("IX_Client_UrlCode");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("ClientId");

                    b.Property<decimal?>("Default_Cost_Per_Unit");

                    b.Property<string>("Desription");

                    b.Property<DateTime?>("End_Date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<DateTime?>("Start_Date");

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_ClientProduct_Active");

                    b.HasIndex("ClientId")
                        .HasName("IX_ClientProduct_Client");

                    b.HasIndex("Active", "ClientId")
                        .HasName("IX_ClientProduct_ClientActive");

                    b.ToTable("ClientProducts");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProductRetailer", b =>
                {
                    b.Property<Guid>("RetailerId");

                    b.Property<Guid>("ClientProductId");

                    b.Property<decimal?>("Cost_Per_Unit");

                    b.Property<string>("Properties");

                    b.HasKey("RetailerId", "ClientProductId")
                        .HasName("PK_ClientProductRetailer");

                    b.HasIndex("ClientProductId");

                    b.ToTable("ClientProductRetailers");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProductStore", b =>
                {
                    b.Property<Guid>("StoreId");

                    b.Property<Guid>("ClientProductId");

                    b.Property<decimal?>("Cost_Per_Unit");

                    b.Property<string>("Properties");

                    b.HasKey("StoreId", "ClientProductId")
                        .HasName("PK_ClientProductStore");

                    b.HasIndex("ClientProductId");

                    b.ToTable("ClientProductStores");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientRetailer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("ClientId");

                    b.Property<string>("Properties");

                    b.Property<Guid>("RetailerId");

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_ClientRetailer_Active");

                    b.HasIndex("ClientId")
                        .HasName("IX_ClientRetailer_Client");

                    b.HasIndex("RetailerId")
                        .IsUnique();

                    b.HasIndex("Active", "ClientId")
                        .HasName("IX_ClientRetailer_ClientActive");

                    b.ToTable("ClientRetailers");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("ClientId");

                    b.Property<string>("Properties");

                    b.Property<Guid>("StoreId");

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_ClientStore_Active");

                    b.HasIndex("ClientId")
                        .HasName("IX_ClientStore_Client");

                    b.HasIndex("StoreId");

                    b.HasIndex("Active", "ClientId")
                        .HasName("IX_ClientStore_ClientActive");

                    b.ToTable("ClientStores");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("ClientId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_ClientUser_Active");

                    b.HasIndex("ClientId")
                        .HasName("IX_ClientUser_Client");

                    b.HasIndex("UserId");

                    b.HasIndex("Active", "ClientId", "UserId")
                        .HasName("IX_ClientUser_ActiveClientUser");

                    b.ToTable("ClientUsers");
                });

            modelBuilder.Entity("tt_apps_srs.Models.Retailer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_Retailer_Active");

                    b.ToTable("Retailers");
                });

            modelBuilder.Entity("tt_apps_srs.Models.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Addr_Ln_1")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("Addr_Ln_2")
                        .HasMaxLength(512);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Country")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4)
                        .HasDefaultValue("US");

                    b.Property<double?>("Latitude");

                    b.Property<int?>("LocationNumber");

                    b.Property<double?>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<Guid>("RetailerId");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.Property<string>("Zip");

                    b.HasKey("Id");

                    b.HasIndex("Active")
                        .HasName("IX_Store_Active");

                    b.HasIndex("RetailerId")
                        .HasName("IX_Store_Retailer");

                    b.HasIndex("Active", "RetailerId")
                        .HasName("IX_Store_RetailerActive");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("tt_apps_srs.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int?>("ClientId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("OpenIdIdentifier")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("Active", "OpenIdIdentifier")
                        .HasName("IX_User_ActiveOpenIdIdentifer");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProduct", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Client", "Client")
                        .WithMany("ClientProducts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProductRetailer", b =>
                {
                    b.HasOne("tt_apps_srs.Models.ClientProduct", "ClientProduct")
                        .WithMany()
                        .HasForeignKey("ClientProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("tt_apps_srs.Models.Retailer", "Retailer")
                        .WithMany()
                        .HasForeignKey("RetailerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientProductStore", b =>
                {
                    b.HasOne("tt_apps_srs.Models.ClientProduct", "ClientProduct")
                        .WithMany()
                        .HasForeignKey("ClientProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("tt_apps_srs.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientRetailer", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Client", "Client")
                        .WithMany("ClientRetailers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("tt_apps_srs.Models.Retailer", "Retailer")
                        .WithOne("ClientRetailer")
                        .HasForeignKey("tt_apps_srs.Models.ClientRetailer", "RetailerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientStore", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Client", "Client")
                        .WithMany("ClientStores")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("tt_apps_srs.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.ClientUser", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Client", "Client")
                        .WithMany("ClientUsers")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("tt_apps_srs.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.Store", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Retailer", "Retailer")
                        .WithMany("Stores")
                        .HasForeignKey("RetailerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("tt_apps_srs.Models.User", b =>
                {
                    b.HasOne("tt_apps_srs.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });
#pragma warning restore 612, 618
        }
    }
}
