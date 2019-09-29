using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBook.DAL.Migrations
{
    public partial class FixRecipeTagForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_RecipeId",
                table: "RecipeTags");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeTags_Tags_TagId",
                table: "RecipeTags");

            migrationBuilder.DropIndex(
                name: "IX_RecipeTags_TagId",
                table: "RecipeTags");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeTags_Tags_RecipeId",
                table: "RecipeTags",
                column: "RecipeId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
