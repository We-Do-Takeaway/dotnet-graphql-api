using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "basket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basket", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "basket_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qty = table.Column<int>(type: "integer", nullable: false),
                    basket_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basket_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_basket_item_basket_basket_id",
                        column: x => x.basket_id,
                        principalTable: "basket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_basket_item_basket_id",
                table: "basket_item",
                column: "basket_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basket_item");

            migrationBuilder.DropTable(
                name: "basket");
        }
    }
}
