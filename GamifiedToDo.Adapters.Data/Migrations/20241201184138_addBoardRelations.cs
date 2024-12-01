using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamifiedToDo.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class addBoardRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoreIds",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Collaborators",
                table: "Board");

            migrationBuilder.CreateTable(
                name: "BoardChore",
                columns: table => new
                {
                    BoardsId = table.Column<string>(type: "text", nullable: false),
                    ChoresId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardChore", x => new { x.BoardsId, x.ChoresId });
                    table.ForeignKey(
                        name: "FK_BoardChore_Board_BoardsId",
                        column: x => x.BoardsId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardChore_Chore_ChoresId",
                        column: x => x.ChoresId,
                        principalTable: "Chore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardUser",
                columns: table => new
                {
                    CollaborationBoardsId = table.Column<string>(type: "text", nullable: false),
                    CollaboratorsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardUser", x => new { x.CollaborationBoardsId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_BoardUser_Board_CollaborationBoardsId",
                        column: x => x.CollaborationBoardsId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardUser_User_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardChore_ChoresId",
                table: "BoardChore",
                column: "ChoresId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardUser_CollaboratorsId",
                table: "BoardUser",
                column: "CollaboratorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardChore");

            migrationBuilder.DropTable(
                name: "BoardUser");

            migrationBuilder.AddColumn<string>(
                name: "ChoreIds",
                table: "Board",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Collaborators",
                table: "Board",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
