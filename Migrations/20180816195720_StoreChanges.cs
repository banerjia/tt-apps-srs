using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tt_apps_srs.Migrations
{
    public partial class StoreChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Retailers_RetailerId1",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_RetailerId1",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "RetailerId1",
                table: "Stores");

            migrationBuilder.AlterColumn<float>(
                name: "Longitude",
                table: "Stores",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<float>(
                name: "Latitude",
                table: "Stores",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<int>(
                name: "LocationNumber",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Stores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationNumber",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Stores");

            migrationBuilder.AlterColumn<float>(
                name: "Longitude",
                table: "Stores",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Latitude",
                table: "Stores",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RetailerId1",
                table: "Stores",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_RetailerId1",
                table: "Stores",
                column: "RetailerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Retailers_RetailerId1",
                table: "Stores",
                column: "RetailerId1",
                principalTable: "Retailers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
