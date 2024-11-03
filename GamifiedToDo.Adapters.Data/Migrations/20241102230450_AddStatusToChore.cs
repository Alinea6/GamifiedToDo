using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamifiedToDo.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToChore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Chore",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Chore");
        }
    }
}
