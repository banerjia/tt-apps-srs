using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class ClientStoreOrderUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrder_ClientStores_ClientStoreId",
                table: "ClientStoreOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProduct_ClientStoreOrder_OrderId",
                table: "ClientStoreOrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProduct_ClientProducts_ProductId",
                table: "ClientStoreOrderProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientStoreOrder",
                table: "ClientStoreOrder");

            migrationBuilder.RenameTable(
                name: "ClientStoreOrderProduct",
                newName: "ClientStoreOrderProducts");

            migrationBuilder.RenameTable(
                name: "ClientStoreOrder",
                newName: "ClientStoreOrders");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStoreOrderProduct_ProductId",
                table: "ClientStoreOrderProducts",
                newName: "IX_ClientStoreOrderProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStoreOrder_ClientStoreId",
                table: "ClientStoreOrders",
                newName: "IX_ClientStoreOrders_ClientStoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientStoreOrders",
                table: "ClientStoreOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientStoreOrders_OrderId",
                table: "ClientStoreOrderProducts",
                column: "OrderId",
                principalTable: "ClientStoreOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientProducts_ProductId",
                table: "ClientStoreOrderProducts",
                column: "ProductId",
                principalTable: "ClientProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrders_ClientStores_ClientStoreId",
                table: "ClientStoreOrders",
                column: "ClientStoreId",
                principalTable: "ClientStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientStoreOrders_OrderId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrderProducts_ClientProducts_ProductId",
                table: "ClientStoreOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStoreOrders_ClientStores_ClientStoreId",
                table: "ClientStoreOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientStoreOrders",
                table: "ClientStoreOrders");

            migrationBuilder.RenameTable(
                name: "ClientStoreOrders",
                newName: "ClientStoreOrder");

            migrationBuilder.RenameTable(
                name: "ClientStoreOrderProducts",
                newName: "ClientStoreOrderProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStoreOrders_ClientStoreId",
                table: "ClientStoreOrder",
                newName: "IX_ClientStoreOrder_ClientStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStoreOrderProducts_ProductId",
                table: "ClientStoreOrderProduct",
                newName: "IX_ClientStoreOrderProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientStoreOrder",
                table: "ClientStoreOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrder_ClientStores_ClientStoreId",
                table: "ClientStoreOrder",
                column: "ClientStoreId",
                principalTable: "ClientStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProduct_ClientStoreOrder_OrderId",
                table: "ClientStoreOrderProduct",
                column: "OrderId",
                principalTable: "ClientStoreOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStoreOrderProduct_ClientProducts_ProductId",
                table: "ClientStoreOrderProduct",
                column: "ProductId",
                principalTable: "ClientProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
