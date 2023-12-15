using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class LotUserBuyer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserBuyerId",
                table: "Lots",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_UserBuyerId",
                table: "Lots",
                column: "UserBuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_UserBuyerId",
                table: "Lots",
                column: "UserBuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_UserBuyerId",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_UserBuyerId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "UserBuyerId",
                table: "Lots");
        }
    }
}
