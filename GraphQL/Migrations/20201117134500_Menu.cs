using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    public partial class MenuSectionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_basket_item",
                table: "basket_item");

            migrationBuilder.DropColumn(
                name: "id",
                table: "basket_item");

            migrationBuilder.AddPrimaryKey(
                name: "PK_basket_item",
                table: "basket_item",
                columns: new[] { "item_id", "basket_id" });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    introduction = table.Column<string>(type: "text", nullable: true),
                    footer = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "item_ingredient",
                columns: table => new
                {
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_ingredient", x => new { x.item_id, x.ingredient_id });
                    table.ForeignKey(
                        name: "FK_item_ingredient_ingredient_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_ingredient_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "section",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    introduction = table.Column<string>(type: "text", nullable: true),
                    footer = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "section_item",
                columns: table => new
                {
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section_item", x => new { x.section_id, x.item_id });
                    table.ForeignKey(
                        name: "FK_section_item_item_item_id",
                        column: x => x.item_id,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_section_item_section_section_id",
                        column: x => x.section_id,
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_ingredient_ingredient_id",
                table: "item_ingredient",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_menu_id",
                table: "section",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_item_item_id",
                table: "section_item",
                column: "item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_basket_item_item_item_id",
                table: "basket_item",
                column: "item_id",
                principalTable: "item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
            
            #region Seed Data
            // Menu
            var menuId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea1");
            
            var mainSectionId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea2");
            var desertSectionId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea3");
            
            var plateOfSausagesId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea4");
            var chocIceCreamSurpriseId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea5");
            var bowlOfCherriesId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea6");

            var sausageId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea7");
            var cherryId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea8");
            var iceCreamId = Guid.Parse("600dca30-c6e2-4035-ad15-783c122d6ea9");
            
            migrationBuilder.InsertData(
                table: "menu",
                columns: new[] { "id","name", "description", "photo" },
                values: new object[]
                {
                    menuId, 
                    "Cafe comfort and waffles", 
                    "The cafe is all about food to make you feel better when you can't face the world or feel like you are going crazy. Have an all day breakfast, your mothers Sunday roast or help yourself to the biggest bowl of ice-cream will lots of extras",
                    "https://www.wedotakeaway.com/images/kitchen.jpg"
                }
            );
            
            // Sections
            migrationBuilder.InsertData(
                table: "section",
                columns: new[] { "id","name", "description", "menu_id", "display_order" },
                values: new object[]
                {
                    mainSectionId, 
                    "Main", 
                    "Stuff to fill you up",
                    menuId,
                    1
                }
                
            );
            
            migrationBuilder.InsertData(
                table: "section",
                columns: new[] { "id","name", "description", "menu_id", "display_order" },
                values: new object[]
                {
                    desertSectionId, 
                    "Desert", 
                    "The best stuff",
                    menuId,
                    2
                }
            );
            
            // Plate of sausages
            migrationBuilder.InsertData(
                table: "item",
                columns: new[] { "id","name", "description", "photo" },
                values: new object[]
                {
                    plateOfSausagesId, 
                    "Plate of sausages", 
                    "Big bowl of sausages",
                    "https://www.wedotakeaway.com/images/sausages.jpg"
                }
            );
            
            // Join it to the section
            migrationBuilder.InsertData(
                table: "section_item",
                columns: new[] { "section_id", "item_id", "display_order" },
                values: new object[]
                {
                    mainSectionId, 
                    plateOfSausagesId,
                    1
                }
            );
            
            // Add the ingredient
            migrationBuilder.InsertData(
                table: "ingredient",
                columns: new[] { "id","name", "description", "photo" },
                values: new object[]
                {
                    sausageId, 
                    "Sausage", 
                    "Locally reared",
                    "https://www.wedotakeaway.com/images/sausages.jpg"
                }
            );
            
            // And say we need 4 of the ingredient
            migrationBuilder.InsertData(
                table: "item_ingredient",
                columns: new[] { "item_id", "ingredient_id", "quantity" },
                values: new object[]
                {
                    plateOfSausagesId,
                    sausageId, 
                    5
                }
            );
            
            //////////////
            // Bowl of cherries
            migrationBuilder.InsertData(
                table: "item",
                columns: new[] { "id","name", "description", "photo" },
                values: new object[]
                {
                    bowlOfCherriesId, 
                    "Bowl of cherries", 
                    "Big bowl of cherries",
                    "https://www.wedotakeaway.com/images/cherries.jpg"
                }
            );
            
            // Join it to the section
            migrationBuilder.InsertData(
                table: "section_item",
                columns: new[] { "section_id", "item_id", "display_order" },
                values: new object[]
                {
                    desertSectionId, 
                    bowlOfCherriesId,
                    1
                }
            );
            
            // Add the ingredient
            migrationBuilder.InsertData(
                table: "ingredient",
                columns: new[] { "id","name", "description" },
                values: new object[]
                {
                    cherryId, 
                    "Cherry", 
                    "In a jar",
                }
            );
            
            // And say we need 2 of the ingredient
            migrationBuilder.InsertData(
                table: "item_ingredient",
                columns: new[] { "item_id", "ingredient_id", "quantity" },
                values: new object[]
                {
                    bowlOfCherriesId,
                    cherryId, 
                    2
                }
            );
            
            //////////////
            // Ice Cream surprise
            migrationBuilder.InsertData(
                table: "item",
                columns: new[] { "id","name", "description", "photo" },
                values: new object[]
                {
                    chocIceCreamSurpriseId, 
                    "Chocolate ice-cream surprise", 
                    "An amazing mixture of chocolate and cherries",
                    "https://www.wedotakeaway.com/images/choc-ice-cream.webp"
                }
            );
            
            // Join it to the section
            migrationBuilder.InsertData(
                table: "section_item",
                columns: new[] { "section_id", "item_id", "display_order" },
                values: new object[]
                {
                    desertSectionId, 
                    chocIceCreamSurpriseId,
                    2
                }
            );
            
            // Add the ingredient
            migrationBuilder.InsertData(
                table: "ingredient",
                columns: new[] { "id","name", "description" },
                values: new object[]
                {
                    iceCreamId, 
                    "Chocolate ice cream", 
                    "Rich yummy belgium chocolate",
                }
            );
            
            // And say we need ice cream and cherries
            migrationBuilder.InsertData(
                table: "item_ingredient",
                columns: new[] { "item_id", "ingredient_id", "quantity" },
                values: new object[]
                {
                    chocIceCreamSurpriseId,
                    iceCreamId, 
                    3
                }
            );
            
            migrationBuilder.InsertData(
                table: "item_ingredient",
                columns: new[] { "item_id", "ingredient_id", "quantity" },
                values: new object[]
                {
                    chocIceCreamSurpriseId,
                    cherryId, 
                    2
                }
            );
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_basket_item_item_item_id",
                table: "basket_item");

            migrationBuilder.DropTable(
                name: "item_ingredient");

            migrationBuilder.DropTable(
                name: "section_item");

            migrationBuilder.DropTable(
                name: "ingredient");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "section");

            migrationBuilder.DropTable(
                name: "menu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_basket_item",
                table: "basket_item");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "basket_item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_basket_item",
                table: "basket_item",
                column: "id");
        }
    }
}
