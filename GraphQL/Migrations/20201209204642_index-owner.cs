using Microsoft.EntityFrameworkCore.Migrations;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    public partial class indexowner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_basket_owner_id",
                table: "basket",
                column: "owner_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_basket_owner_id",
                table: "basket");
        }
    }
}
