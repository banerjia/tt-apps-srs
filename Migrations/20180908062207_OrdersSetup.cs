using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class OrdersSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProductRetailers");

            migrationBuilder.DropTable(
                name: "ClientProductStores");

            migrationBuilder.CreateTable(
                name: "ClientStoreOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientStoreId = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 4, nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(6)", nullable: false),
                    VerifiedBy = table.Column<Guid>(nullable: true),
                    VerifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStoreOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientStoreOrder_ClientStores_ClientStoreId",
                        column: x => x.ClientStoreId,
                        principalTable: "ClientStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientStoreOrderProduct",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<uint>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStoreOrderProduct", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ClientStoreOrderProduct_ClientStoreOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ClientStoreOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientStoreOrderProduct_ClientProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ClientProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrder_ClientStoreId",
                table: "ClientStoreOrder",
                column: "ClientStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrder_Status",
                table: "ClientStoreOrder",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrderProduct_ProductId",
                table: "ClientStoreOrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrderProduct_Status",
                table: "ClientStoreOrderProduct",
                columns: new[] { "OrderId", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientStoreOrderProduct");

            migrationBuilder.DropTable(
                name: "ClientStoreOrder");

            migrationBuilder.CreateTable(
                name: "ClientProductRetailers",
                columns: table => new
                {
                    RetailerId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false),
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<string>(type: "JSON", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProductRetailer", x => new { x.RetailerId, x.ClientProductId });
                    table.ForeignKey(
                        name: "FK_ClientProductRetailers_ClientProducts_ClientProductId",
                        column: x => x.ClientProductId,
                        principalTable: "ClientProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProductRetailers_Retailers_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Retailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientProductStores",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false),
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<string>(type: "JSON", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProductStore", x => new { x.StoreId, x.ClientProductId });
                    table.ForeignKey(
                        name: "FK_ClientProductStores_ClientProducts_ClientProductId",
                        column: x => x.ClientProductId,
                        principalTable: "ClientProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProductStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProductRetailers_ClientProductId",
                table: "ClientProductRetailers",
                column: "ClientProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProductStores_ClientProductId",
                table: "ClientProductStores",
                column: "ClientProductId");
        }
    }
}
