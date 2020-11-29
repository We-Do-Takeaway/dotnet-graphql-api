using Microsoft.EntityFrameworkCore.Migrations;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    public partial class Quantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "qty",
                table: "basket_item",
                newName: "quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "basket_item",
                newName: "qty");
        }
    }
}
