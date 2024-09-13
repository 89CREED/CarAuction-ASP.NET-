using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutaCH_MD.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bids");
        }
    }
}
