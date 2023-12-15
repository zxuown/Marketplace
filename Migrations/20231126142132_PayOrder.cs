using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class PayOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Uid = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayOrders_UserId",
                table: "PayOrders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayOrders");
        }
    }
}
