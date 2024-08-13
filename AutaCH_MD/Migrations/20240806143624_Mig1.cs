using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutaCH_MD.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Cars_CarId1",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarId1",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "Cars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarId1",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarId1",
                table: "Cars",
                column: "CarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Cars_CarId1",
                table: "Cars",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "CarId");
        }
    }
}
