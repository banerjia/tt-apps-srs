using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tt_apps_srs.Migrations
{
    public partial class ClientStoreCreatedAtTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Properties",
                table: "ClientStores",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ClientStores",
                type: "TIMESTAMP(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)");

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
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ClientStores");

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
