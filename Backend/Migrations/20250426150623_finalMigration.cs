using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class finalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competition_AspNetUsers_CreatorId",
                table: "Competition");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Competition_CompetitionId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Moderators_Competition_CompetitionId",
                table: "Moderators");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Competition_CompetitionId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "InvitedSubmissionUsers");

            migrationBuilder.DropTable(
                name: "OpenGroupJoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Competition",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Competition");

            migrationBuilder.RenameTable(
                name: "Competition",
                newName: "Competitions");

            migrationBuilder.RenameIndex(
                name: "IX_Competition_CreatorId",
                table: "Competitions",
                newName: "IX_Competitions_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GroupInvites",
                columns: table => new
                {
                    RecieverUserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SenderUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInvites", x => new { x.RecieverUserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupInvites_AspNetUsers_RecieverUserId",
                        column: x => x.RecieverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInvites_AspNetUsers_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInvites_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    RequestOwnerId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => new { x.RequestOwnerId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_JoinRequests_AspNetUsers_RequestOwnerId",
                        column: x => x.RequestOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinRequests_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvites_GroupId",
                table: "GroupInvites",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInvites_SenderUserId",
                table: "GroupInvites",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_GroupId",
                table: "JoinRequests",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_AspNetUsers_CreatorId",
                table: "Competitions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Competitions_CompetitionId",
                table: "Groups",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Moderators_Competitions_CompetitionId",
                table: "Moderators",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Competitions_CompetitionId",
                table: "Submissions",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_AspNetUsers_CreatorId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Competitions_CompetitionId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Moderators_Competitions_CompetitionId",
                table: "Moderators");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Competitions_CompetitionId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "GroupInvites");

            migrationBuilder.DropTable(
                name: "JoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions");

            migrationBuilder.RenameTable(
                name: "Competitions",
                newName: "Competition");

            migrationBuilder.RenameIndex(
                name: "IX_Competitions_CreatorId",
                table: "Competition",
                newName: "IX_Competition_CreatorId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Competition",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competition",
                table: "Competition",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InvitedSubmissionUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RecieverUserId = table.Column<int>(type: "int", nullable: false),
                    SenderUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitedSubmissionUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitedSubmissionUsers_AspNetUsers_RecieverUserId",
                        column: x => x.RecieverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvitedSubmissionUsers_AspNetUsers_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvitedSubmissionUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OpenGroupJoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RequestOwnerId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenGroupJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenGroupJoinRequests_AspNetUsers_RequestOwnerId",
                        column: x => x.RequestOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenGroupJoinRequests_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InvitedSubmissionUsers_GroupId",
                table: "InvitedSubmissionUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitedSubmissionUsers_RecieverUserId",
                table: "InvitedSubmissionUsers",
                column: "RecieverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitedSubmissionUsers_SenderUserId",
                table: "InvitedSubmissionUsers",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenGroupJoinRequests_GroupId",
                table: "OpenGroupJoinRequests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenGroupJoinRequests_RequestOwnerId",
                table: "OpenGroupJoinRequests",
                column: "RequestOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competition_AspNetUsers_CreatorId",
                table: "Competition",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Competition_CompetitionId",
                table: "Groups",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Moderators_Competition_CompetitionId",
                table: "Moderators",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Competition_CompetitionId",
                table: "Submissions",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
