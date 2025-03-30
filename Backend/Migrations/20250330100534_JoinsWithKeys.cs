using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class JoinsWithKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionUser");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "SubmissionUser");

            migrationBuilder.DropTable(
                name: "SubmissionUser1");

            migrationBuilder.AddColumn<bool>(
                name: "IsShortStoryCompetition",
                table: "Competition",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CollaboratedSubmissions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratedSubmissions", x => new { x.UserId, x.SubmissionId });
                    table.ForeignKey(
                        name: "FK_CollaboratedSubmissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratedSubmissions_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Moderators",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderators", x => new { x.UserId, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_Moderators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Moderators_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubmissionVotes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionVotes", x => new { x.UserId, x.SubmissionId });
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionVotes_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserParticipateGroups",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserParticipateGroups", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserParticipateGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserParticipateGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratedSubmissions_SubmissionId",
                table: "CollaboratedSubmissions",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Moderators_CompetitionId",
                table: "Moderators",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionVotes_SubmissionId",
                table: "SubmissionVotes",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserParticipateGroups_GroupId",
                table: "UserParticipateGroups",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratedSubmissions");

            migrationBuilder.DropTable(
                name: "Moderators");

            migrationBuilder.DropTable(
                name: "SubmissionVotes");

            migrationBuilder.DropTable(
                name: "UserParticipateGroups");

            migrationBuilder.DropColumn(
                name: "IsShortStoryCompetition",
                table: "Competition");

            migrationBuilder.CreateTable(
                name: "CompetitionUser",
                columns: table => new
                {
                    ModeratedCompetitionsId = table.Column<int>(type: "int", nullable: false),
                    ModeratorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionUser", x => new { x.ModeratedCompetitionsId, x.ModeratorsId });
                    table.ForeignKey(
                        name: "FK_CompetitionUser_AspNetUsers_ModeratorsId",
                        column: x => x.ModeratorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionUser_Competition_ModeratedCompetitionsId",
                        column: x => x.ModeratedCompetitionsId,
                        principalTable: "Competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "int", nullable: false),
                    ParticipatingGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.ParticipantsId, x.ParticipatingGroupsId });
                    table.ForeignKey(
                        name: "FK_GroupUser_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_ParticipatingGroupsId",
                        column: x => x.ParticipatingGroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubmissionUser",
                columns: table => new
                {
                    CollaboratedSubmissionsId = table.Column<int>(type: "int", nullable: false),
                    CreatorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionUser", x => new { x.CollaboratedSubmissionsId, x.CreatorsId });
                    table.ForeignKey(
                        name: "FK_SubmissionUser_AspNetUsers_CreatorsId",
                        column: x => x.CreatorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionUser_Submissions_CollaboratedSubmissionsId",
                        column: x => x.CollaboratedSubmissionsId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_CompetitionUser_ModeratorsId",
                table: "CompetitionUser",
                column: "ModeratorsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_ParticipatingGroupsId",
                table: "GroupUser",
                column: "ParticipatingGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionUser_CreatorsId",
                table: "SubmissionUser",
                column: "CreatorsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionUser1_VotedUsersId",
                table: "SubmissionUser1",
                column: "VotedUsersId");
        }
    }
}
