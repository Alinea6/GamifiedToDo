using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamifiedToDo.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class addUserLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Chore",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Exp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLevel_Id",
                table: "UserLevel",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserLevel_Id",
                table: "User",
                column: "Id",
                principalTable: "UserLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserLevel_Id",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserLevel");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Chore");
        }
    }
}
