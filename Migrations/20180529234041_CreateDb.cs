using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tt_apps_srs.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Retailers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Addr_Ln_1 = table.Column<string>(maxLength: 1024, nullable: false),
                    Addr_Ln_2 = table.Column<string>(maxLength: 512, nullable: true),
                    City = table.Column<string>(maxLength: 128, nullable: false),
                    Country = table.Column<string>(maxLength: 4, nullable: false, defaultValue: "US"),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    State = table.Column<string>(maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantRetailers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true),
                    RetailerId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantRetailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true),
                    UrlCode = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantStores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantStores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantRetailer_TenantActive",
                table: "TenantRetailers",
                columns: new[] { "Active", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_UrlCode",
                table: "Tenants",
                column: "UrlCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantStore_TenantActive",
                table: "TenantStores",
                columns: new[] { "Active", "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Retailers");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "TenantRetailers");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "TenantStores");
        }
    }
}
