using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBook.DAL.Migrations
{
    public partial class EnumImprovements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeStatuses_RecipeStatusId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_UserStatuses_UserStatusId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "RecipeStatuses");

            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UserStatusId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_RecipeStatusId",
                table: "Recipes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "UserStatusId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RecipeStatusId",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "UserStatus",
                table: "UserProfiles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipeStatus",
                table: "Recipes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "RecipeStatus",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "UserStatusId",
                table: "UserProfiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipeStatusId",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipeStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeStatuses", x => x.Id);
                    table.UniqueConstraint("AK_RecipeStatuses_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                    table.UniqueConstraint("AK_UserStatuses_Name", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "e7fa7b62-1446-4582-b707-4603f647eeb4", "Admin", "ADMIN" },
                    { 2, "e8271b65-8ac3-47c3-bacf-c0b77adf5627", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "RecipeStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "UserStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Pending" },
                    { 3, "Blocked" },
                    { 4, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserStatusId",
                table: "UserProfiles",
                column: "UserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeStatusId",
                table: "Recipes",
                column: "RecipeStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeStatuses_RecipeStatusId",
                table: "Recipes",
                column: "RecipeStatusId",
                principalTable: "RecipeStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_UserStatuses_UserStatusId",
                table: "UserProfiles",
                column: "UserStatusId",
                principalTable: "UserStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
