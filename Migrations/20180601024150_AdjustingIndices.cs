using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tt_apps_srs.Migrations
{
    public partial class AdjustingIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientId",
                table: "Users",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Active",
                table: "Stores",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Retailer",
                table: "Stores",
                column: "RetailerId");

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
                name: "IX_ClientProductStores_ClientProductId",
                table: "ClientProductStores",
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
                name: "IX_ClientProductRetailers_ClientProductId",
                table: "ClientProductRetailers",
                column: "ClientProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProductRetailers_ClientProducts_ClientProductId",
                table: "ClientProductRetailers",
                column: "ClientProductId",
                principalTable: "ClientProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProductRetailers_Retailers_RetailerId",
                table: "ClientProductRetailers",
                column: "RetailerId",
                principalTable: "Retailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProducts_Clients_ClientId",
                table: "ClientProducts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProductStores_ClientProducts_ClientProductId",
                table: "ClientProductStores",
                column: "ClientProductId",
                principalTable: "ClientProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProductStores_Stores_StoreId",
                table: "ClientProductStores",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRetailers_Clients_ClientId",
                table: "ClientRetailers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRetailers_Retailers_RetailerId",
                table: "ClientRetailers",
                column: "RetailerId",
                principalTable: "Retailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStores_Clients_ClientId",
                table: "ClientStores",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStores_Stores_StoreId",
                table: "ClientStores",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientUsers_Clients_ClientId",
                table: "ClientUsers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientUsers_Users_UserId",
                table: "ClientUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Retailers_RetailerId",
                table: "Stores",
                column: "RetailerId",
                principalTable: "Retailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProductRetailers_ClientProducts_ClientProductId",
                table: "ClientProductRetailers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProductRetailers_Retailers_RetailerId",
                table: "ClientProductRetailers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProducts_Clients_ClientId",
                table: "ClientProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProductStores_ClientProducts_ClientProductId",
                table: "ClientProductStores");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProductStores_Stores_StoreId",
                table: "ClientProductStores");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientRetailers_Clients_ClientId",
                table: "ClientRetailers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientRetailers_Retailers_RetailerId",
                table: "ClientRetailers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStores_Clients_ClientId",
                table: "ClientStores");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStores_Stores_StoreId",
                table: "ClientStores");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientUsers_Clients_ClientId",
                table: "ClientUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientUsers_Users_UserId",
                table: "ClientUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Retailers_RetailerId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Clients_ClientId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClientId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Store_Active",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Store_Retailer",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_ClientUser_Active",
                table: "ClientUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClientUser_Client",
                table: "ClientUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClientUsers_UserId",
                table: "ClientUsers");

            migrationBuilder.DropIndex(
                name: "IX_ClientStore_Active",
                table: "ClientStores");

            migrationBuilder.DropIndex(
                name: "IX_ClientStore_Client",
                table: "ClientStores");

            migrationBuilder.DropIndex(
                name: "IX_ClientStores_StoreId",
                table: "ClientStores");

            migrationBuilder.DropIndex(
                name: "IX_ClientRetailer_Active",
                table: "ClientRetailers");

            migrationBuilder.DropIndex(
                name: "IX_ClientRetailer_Client",
                table: "ClientRetailers");

            migrationBuilder.DropIndex(
                name: "IX_ClientRetailers_RetailerId",
                table: "ClientRetailers");

            migrationBuilder.DropIndex(
                name: "IX_ClientProductStores_ClientProductId",
                table: "ClientProductStores");

            migrationBuilder.DropIndex(
                name: "IX_ClientProduct_Active",
                table: "ClientProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientProduct_Client",
                table: "ClientProducts");

            migrationBuilder.DropIndex(
                name: "IX_ClientProductRetailers_ClientProductId",
                table: "ClientProductRetailers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");
        }
    }
}
