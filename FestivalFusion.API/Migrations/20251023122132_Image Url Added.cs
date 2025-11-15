using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FestivalFusion.API.Migrations
{
    /// <inheritdoc />
    public partial class ImageUrlAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VenueImageUrl",
                table: "Venues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArtistImageUrl",
                table: "Artists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenueImageUrl",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ArtistImageUrl",
                table: "Artists");
        }
    }
}
