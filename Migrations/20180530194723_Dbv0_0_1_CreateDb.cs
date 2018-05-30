using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tt_apps_srs.Migrations
{
    public partial class Dbv0_0_1_CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientProductRetailers",
                columns: table => new
                {
                    RetailerId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false),
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProductRetailer", x => new { x.RetailerId, x.ClientProductId });
                });

            migrationBuilder.CreateTable(
                name: "ClientProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Default_Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Desription = table.Column<string>(nullable: true),
                    End_Date = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientProductStores",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false),
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProductStore", x => new { x.StoreId, x.ClientProductId });
                });

            migrationBuilder.CreateTable(
                name: "ClientRetailers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    ClientId = table.Column<int>(nullable: false),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true),
                    RetailerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRetailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
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
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientStores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    ClientId = table.Column<int>(nullable: false),
                    Properties = table.Column<JsonObject<Dictionary<string, object>>>(nullable: true),
                    StoreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStores", x => x.Id);
                });

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
                    RetailerId = table.Column<Guid>(nullable: false),
                    State = table.Column<string>(maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_ClientActive",
                table: "ClientProducts",
                columns: new[] { "Active", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailer_ClientActive",
                table: "ClientRetailers",
                columns: new[] { "Active", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Client_UrlCode",
                table: "Clients",
                column: "UrlCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientStore_ClientActive",
                table: "ClientStores",
                columns: new[] { "Active", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Retailer_Active",
                table: "Retailers",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Store_RetailerActive",
                table: "Stores",
                columns: new[] { "Active", "RetailerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProductRetailers");

            migrationBuilder.DropTable(
                name: "ClientProducts");

            migrationBuilder.DropTable(
                name: "ClientProductStores");

            migrationBuilder.DropTable(
                name: "ClientRetailers");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ClientStores");

            migrationBuilder.DropTable(
                name: "Retailers");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
