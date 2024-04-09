using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreaminandSchemin.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBracketName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BracketSheetName",
                table: "Tournaments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BracketSheetName",
                table: "Tournaments");
        }
    }
}
