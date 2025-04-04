using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedGroupJoinRequests");

            migrationBuilder.CreateTable(
                name: "OpenGroupJoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestOwnerId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_OpenGroupJoinRequests_GroupId",
                table: "OpenGroupJoinRequests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenGroupJoinRequests_RequestOwnerId",
                table: "OpenGroupJoinRequests",
                column: "RequestOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenGroupJoinRequests");

            migrationBuilder.CreateTable(
                name: "ClosedGroupJoinRequests",
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
                    table.PrimaryKey("PK_ClosedGroupJoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClosedGroupJoinRequests_AspNetUsers_RequestOwnerId",
                        column: x => x.RequestOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosedGroupJoinRequests_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedGroupJoinRequests_GroupId",
                table: "ClosedGroupJoinRequests",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedGroupJoinRequests_RequestOwnerId",
                table: "ClosedGroupJoinRequests",
                column: "RequestOwnerId");
        }
    }
}
