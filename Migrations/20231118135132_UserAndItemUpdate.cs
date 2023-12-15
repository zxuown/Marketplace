using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class UserAndItemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "AspNetUsers");
        }
    }
}
