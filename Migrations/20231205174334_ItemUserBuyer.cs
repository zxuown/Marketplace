using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class ItemUserBuyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserBuyerId",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserBuyerId",
                table: "Items",
                column: "UserBuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_UserBuyerId",
                table: "Items",
                column: "UserBuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_UserBuyerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UserBuyerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UserBuyerId",
                table: "Items");
        }
    }
}
