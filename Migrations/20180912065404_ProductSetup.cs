using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class ProductSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientProducts_ProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropTable(
                name: "ClientProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientStoreOrderProduct",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientStoreOrderProducts_ProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "ClientRetailerProductId",
                table: "ClientStoreOrderProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientStoreOrderProduct",
                table: "ClientStoreOrderProducts",
                columns: new[] { "OrderId", "ClientRetailerProductId" });

            migrationBuilder.CreateTable(
                name: "ClientRetailerProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientRetailerId = table.Column<int>(nullable: false),
                    UPC = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Cost = table.Column<decimal>(nullable: true),
                    Properties = table.Column<string>(type: "JSON", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRetailerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRetailerProducts_ClientRetailers_ClientRetailerId",
                        column: x => x.ClientRetailerId,
                        principalTable: "ClientRetailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrderProducts_ClientRetailerProductId",
                table: "ClientStoreOrderProducts",
                column: "ClientRetailerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailerId_UPC",
                table: "ClientRetailerProducts",
                columns: new[] { "ClientRetailerId", "UPC" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientRetailerProducts_ClientRetail~",
                table: "ClientStoreOrderProducts",
                column: "ClientRetailerProductId",
                principalTable: "ClientRetailerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientRetailerProducts_ClientRetail~",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropTable(
                name: "ClientRetailerProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientStoreOrderProduct",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientStoreOrderProducts_ClientRetailerProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropColumn(
                name: "ClientRetailerProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ClientStoreOrderProducts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientStoreOrderProduct",
                table: "ClientStoreOrderProducts",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.CreateTable(
                name: "ClientProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    ClientId = table.Column<int>(nullable: false),
                    Default_Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    End_Date = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_ClientProduct_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrderProducts_ProductId",
                table: "ClientStoreOrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_Active",
                table: "ClientProducts",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_Client",
                table: "ClientProducts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_ClientActive",
                table: "ClientProducts",
                columns: new[] { "Active", "ClientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientProducts_ProductId",
                table: "ClientStoreOrderProducts",
                column: "ProductId",
                principalTable: "ClientProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
