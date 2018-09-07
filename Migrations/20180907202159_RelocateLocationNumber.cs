using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class RelocateLocationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationNumber",
                table: "Stores");

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

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationNumber",
                table: "Stores",
                nullable: true);

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
