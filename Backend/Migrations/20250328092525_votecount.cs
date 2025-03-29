using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class votecount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionVotes_votes_VotesId",
                table: "SubmissionVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVotes_votes_VotesId",
                table: "UserVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_votes",
                table: "votes");

            migrationBuilder.RenameTable(
                name: "votes",
                newName: "Votes");

            migrationBuilder.AddColumn<int>(
                name: "VoteCount",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionVotes_Votes_VotesId",
                table: "SubmissionVotes",
                column: "VotesId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVotes_Votes_VotesId",
                table: "UserVotes",
                column: "VotesId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionVotes_Votes_VotesId",
                table: "SubmissionVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVotes_Votes_VotesId",
                table: "UserVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "VoteCount",
                table: "Submissions");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "votes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_votes",
                table: "votes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionVotes_votes_VotesId",
                table: "SubmissionVotes",
                column: "VotesId",
                principalTable: "votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVotes_votes_VotesId",
                table: "UserVotes",
                column: "VotesId",
                principalTable: "votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
