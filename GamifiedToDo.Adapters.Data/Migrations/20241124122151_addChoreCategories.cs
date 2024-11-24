using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamifiedToDo.Adapters.Data.Migrations
{
    /// <inheritdoc />
    public partial class addChoreCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Chore",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Chore");
        }
    }
}
