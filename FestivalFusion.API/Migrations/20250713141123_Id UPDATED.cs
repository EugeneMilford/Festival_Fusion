using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestivalFusion.API.Migrations
{
    /// <inheritdoc />
    public partial class IdUPDATED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "festivalId",
                table: "Festivals",
                newName: "FestivalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FestivalId",
                table: "Festivals",
                newName: "festivalId");
        }
    }
}
