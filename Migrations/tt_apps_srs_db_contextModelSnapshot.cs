﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
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
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

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
                        .HasDefaultValue("US")
                        .HasMaxLength(4);

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("tt_apps_srs.Models.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<JsonObject<Dictionary<string, object>>>("Properties");

                    b.Property<string>("UrlCode")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("UrlCode")
                        .IsUnique()
                        .HasName("IX_Tenant_UrlCode");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("tt_apps_srs.Models.TenantRetailer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<JsonObject<Dictionary<string, object>>>("Properties");

                    b.Property<Guid>("RetailerId");

                    b.Property<Guid>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("Active", "TenantId")
                        .HasName("IX_TenantRetailer_TenantActive");

                    b.ToTable("TenantRetailers");
                });

            modelBuilder.Entity("tt_apps_srs.Models.TenantStore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<JsonObject<Dictionary<string, object>>>("Properties");

                    b.Property<Guid>("StoreId");

                    b.Property<Guid>("TenantId");

                    b.HasKey("Id");

                    b.HasIndex("Active", "TenantId")
                        .HasName("IX_TenantStore_TenantActive");

                    b.ToTable("TenantStores");
                });
#pragma warning restore 612, 618
        }
    }
}
