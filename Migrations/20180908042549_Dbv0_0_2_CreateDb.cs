using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class Dbv0_0_2_CreateDb : Migration
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
                name: "ClientProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Default_Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Start_Date = table.Column<DateTime>(nullable: true),
                    End_Date = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
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
                    Zip = table.Column<string>(nullable: true),
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
                name: "ClientProductRetailers",
                columns: table => new
                {
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    RetailerId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false)
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
                    Cost_Per_Unit = table.Column<decimal>(nullable: true),
                    Properties = table.Column<string>(type: "JSON", nullable: true),
                    StoreId = table.Column<Guid>(nullable: false),
                    ClientProductId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ClientProductRetailers_ClientProductId",
                table: "ClientProductRetailers",
                column: "ClientProductId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ClientProductStores_ClientProductId",
                table: "ClientProductStores",
                column: "ClientProductId");

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
                name: "ClientProductRetailers");

            migrationBuilder.DropTable(
                name: "ClientProductStores");

            migrationBuilder.DropTable(
                name: "ClientRetailers");

            migrationBuilder.DropTable(
                name: "ClientStores");

            migrationBuilder.DropTable(
                name: "ClientUsers");

            migrationBuilder.DropTable(
                name: "ClientProducts");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Retailers");
        }
    }
}
