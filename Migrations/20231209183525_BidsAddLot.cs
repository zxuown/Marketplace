using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class BidsAddLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "Bids",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_LotId",
                table: "Bids",
                column: "LotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Lots_LotId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_LotId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "Bids");
        }
    }
}
