using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBook.DAL.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "UserProfiles",
                newName: "ImagePublicId");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "ImagePublicId",
                table: "UserProfiles",
                newName: "AvatarUrl");
        }
    }
}
