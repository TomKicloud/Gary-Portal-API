using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class AditLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedAditLogs",
                columns: table => new
                {
                    AditLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AditLogUrl = table.Column<string>(type: "longtext", nullable: true),
                    AditLogThumbnailUrl = table.Column<string>(type: "longtext", nullable: true),
                    PosterUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    AditLogTeamId = table.Column<int>(type: "int", nullable: false),
                    IsVideo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AditLogViews = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(type: "longtext", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedAditLogs", x => x.AditLogId);
                    table.ForeignKey(
                        name: "FK_FeedAditLogs_Teams_AditLogTeamId",
                        column: x => x.AditLogTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedAditLogs_Users_PosterUUID",
                        column: x => x.PosterUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedAditLogs_AditLogTeamId",
                table: "FeedAditLogs",
                column: "AditLogTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedAditLogs_PosterUUID",
                table: "FeedAditLogs",
                column: "PosterUUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedAditLogs");
        }
    }
}
