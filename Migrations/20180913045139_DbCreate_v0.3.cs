using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class DbCreate_v03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    UrlCode = table.Column<string>(maxLength: 64, nullable: false),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Retailers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retailers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    OpenIdIdentifier = table.Column<string>(maxLength: 512, nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientRetailers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    RetailerId = table.Column<Guid>(nullable: false),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRetailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRetailers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRetailers_Retailers_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Retailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RetailerId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Addr_Ln_1 = table.Column<string>(maxLength: 1024, nullable: false),
                    Addr_Ln_2 = table.Column<string>(maxLength: 512, nullable: true),
                    City = table.Column<string>(maxLength: 128, nullable: false),
                    State = table.Column<string>(maxLength: 4, nullable: false),
                    Zip = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 4, nullable: false, defaultValue: "US"),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Retailers_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Retailers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUsers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRetailerProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientRetailerId = table.Column<int>(nullable: false),
                    UPC = table.Column<string>(maxLength: 50, nullable: true),
                    SKU = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Cost = table.Column<decimal>(type: "DECIMAL(7,2)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "ClientStores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    StoreId = table.Column<Guid>(nullable: false),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientStores_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientStoreOrders",
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
                    VerifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStoreOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientStoreOrders_ClientStores_ClientStoreId",
                        column: x => x.ClientStoreId,
                        principalTable: "ClientStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientStoreOrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    ClientRetailerProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<uint>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "DECIMAL(7,2)", nullable: false),
                    Status = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStoreOrderProduct", x => new { x.OrderId, x.ClientRetailerProductId });
                    table.ForeignKey(
                        name: "FK_ClientStoreOrderProducts_ClientRetailerProducts_ClientRetail~",
                        column: x => x.ClientRetailerProductId,
                        principalTable: "ClientRetailerProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientStoreOrderProducts_ClientStoreOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ClientStoreOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailerId_UPC_SKU",
                table: "ClientRetailerProducts",
                columns: new[] { "ClientRetailerId", "UPC", "SKU" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailer_Active",
                table: "ClientRetailers",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailer_Client",
                table: "ClientRetailers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRetailers_RetailerId",
                table: "ClientRetailers",
                column: "RetailerId");

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
                name: "IX_ClientStoreOrderProducts_ClientRetailerProductId",
                table: "ClientStoreOrderProducts",
                column: "ClientRetailerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrderProduct_Status",
                table: "ClientStoreOrderProducts",
                columns: new[] { "OrderId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrders_ClientStoreId",
                table: "ClientStoreOrders",
                column: "ClientStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStoreOrder_Status",
                table: "ClientStoreOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStore_Active",
                table: "ClientStores",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStore_Client",
                table: "ClientStores",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStores_StoreId",
                table: "ClientStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStore_ClientActive",
                table: "ClientStores",
                columns: new[] { "Active", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_Active",
                table: "ClientUsers",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_Client",
                table: "ClientUsers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUsers_UserId",
                table: "ClientUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_ActiveClientUser",
                table: "ClientUsers",
                columns: new[] { "Active", "ClientId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Retailer_Active",
                table: "Retailers",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Active",
                table: "Stores",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Retailer",
                table: "Stores",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_RetailerActive",
                table: "Stores",
                columns: new[] { "Active", "RetailerId" });

            migrationBuilder.CreateIndex(
                name: "IX_User_ActiveOpenIdIdentifer",
                table: "Users",
                columns: new[] { "Active", "OpenIdIdentifier" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientStoreOrderProducts");

            migrationBuilder.DropTable(
                name: "ClientUsers");

            migrationBuilder.DropTable(
                name: "ClientRetailerProducts");

            migrationBuilder.DropTable(
                name: "ClientStoreOrders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientRetailers");

            migrationBuilder.DropTable(
                name: "ClientStores");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Retailers");
        }
    }
}
