using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamifiedToDo.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class addFriendRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequest_User_FriendId",
                        column: x => x.FriendId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_FriendId",
                table: "FriendRequest",
                column: "FriendId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_Id",
                table: "FriendRequest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_UserId",
                table: "FriendRequest",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequest");
        }
    }
}
