using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace tt_apps_srs.Migrations
{
    public partial class KeyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
