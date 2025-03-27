using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class DetailsReworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MusicDetails");

            migrationBuilder.DropColumn(
                name: "MovieLink",
                table: "MusicDetails");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MusicDetails");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MovieDetails");

            migrationBuilder.RenameColumn(
                name: "TrailerLink",
                table: "MusicDetails",
                newName: "Link");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MovieDetails",
                newName: "TrailerLink");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "MovieDetails",
                newName: "MovieLink");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Submissions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "MusicDetails",
                newName: "TrailerLink");

            migrationBuilder.RenameColumn(
                name: "TrailerLink",
                table: "MovieDetails",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "MovieLink",
                table: "MovieDetails",
                newName: "Link");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MusicDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MovieLink",
                table: "MusicDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MusicDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MovieDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
