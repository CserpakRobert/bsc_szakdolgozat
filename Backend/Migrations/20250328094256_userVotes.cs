using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class userVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionVotes");

            migrationBuilder.DropTable(
                name: "UserVotes");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.CreateTable(
                name: "SubmissionUser1",
                columns: table => new
                {
                    VotedSubmissionsId = table.Column<int>(type: "int", nullable: false),
                    VotedUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionUser1", x => new { x.VotedSubmissionsId, x.VotedUsersId });
                    table.ForeignKey(
                        name: "FK_SubmissionUser1_AspNetUsers_VotedUsersId",
                        column: x => x.VotedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionUser1_Submissions_VotedSubmissionsId",
                        column: x => x.VotedSubmissionsId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionUser1_VotedUsersId",
                table: "SubmissionUser1",
                column: "VotedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionUser1");

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubmissionVotes",
                columns: table => new
                {
                    SubmissionsId = table.Column<int>(type: "int", nullable: false),
                    VotesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionVotes", x => new { x.SubmissionsId, x.VotesId });
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_Submissions_SubmissionsId",
                        column: x => x.SubmissionsId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_Votes_VotesId",
                        column: x => x.VotesId,
                        principalTable: "Votes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserVotes",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "int", nullable: false),
                    VotesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVotes", x => new { x.UsersId, x.VotesId });
                    table.ForeignKey(
                        name: "FK_UserVotes_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVotes_Votes_VotesId",
                        column: x => x.VotesId,
                        principalTable: "Votes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionVotes_VotesId",
                table: "SubmissionVotes",
                column: "VotesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_VotesId",
                table: "UserVotes",
                column: "VotesId");
        }
    }
}
