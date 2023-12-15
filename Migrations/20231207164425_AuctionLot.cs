using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class AuctionLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinalDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageId = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lots_ImageFiles_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ImageFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lots_ImageId",
                table: "Lots",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_UserId",
                table: "Lots",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lots");
        }
    }
}
