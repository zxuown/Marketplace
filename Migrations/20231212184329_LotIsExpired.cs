using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class LotIsExpired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "Lots",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "Lots");
        }
    }
}
