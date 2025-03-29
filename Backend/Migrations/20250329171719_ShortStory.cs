using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class ShortStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Placement",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShortStoryDetailsId",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ClosedGroupJoinRequests",
                type: "varchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShortStoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Story = table.Column<string>(type: "longtext", maxLength: 20000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortStoryDetails", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ShortStoryDetailsId",
                table: "Submissions",
                column: "ShortStoryDetailsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ShortStoryDetails_ShortStoryDetailsId",
                table: "Submissions",
                column: "ShortStoryDetailsId",
                principalTable: "ShortStoryDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ShortStoryDetails_ShortStoryDetailsId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "ShortStoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ShortStoryDetailsId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Placement",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "ShortStoryDetailsId",
                table: "Submissions");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ClosedGroupJoinRequests",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)",
                oldMaxLength: 3000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
