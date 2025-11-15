using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestivalFusion.API.Migrations
{
    /// <inheritdoc />
    public partial class FestivalImageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FestivalImageUrl",
                table: "Festivals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FestivalImageUrl",
                table: "Festivals");
        }
    }
}
