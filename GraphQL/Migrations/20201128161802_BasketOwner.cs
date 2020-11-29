using Microsoft.EntityFrameworkCore.Migrations;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    public partial class BasketOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "basket",
                newName: "owner_id");

            migrationBuilder.AddColumn<string>(
                name: "basket_type",
                table: "basket",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "basket_type",
                table: "basket");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "basket",
                newName: "customer_id");
        }
    }
}
