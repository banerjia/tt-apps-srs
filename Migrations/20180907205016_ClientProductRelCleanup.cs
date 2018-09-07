using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class ClientProductRelCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desription",
                table: "ClientProducts",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientStores",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientRetailers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientProductStores",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientProductRetailers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ClientProducts",
                newName: "Desription");

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientStores",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientRetailers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientProductStores",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientProductRetailers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
