using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class Reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedReports",
                columns: table => new
                {
                    FeedReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FeedPostId = table.Column<int>(type: "int", nullable: false),
                    ReportReason = table.Column<string>(type: "longtext", nullable: true),
                    ReportIssuedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReportByUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedReports", x => x.FeedReportId);
                    table.ForeignKey(
                        name: "FK_FeedReports_FeedPosts_FeedPostId",
                        column: x => x.FeedPostId,
                        principalTable: "FeedPosts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedReports_Users_ReportByUUID",
                        column: x => x.ReportByUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReports",
                columns: table => new
                {
                    UserReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ReportReason = table.Column<string>(type: "longtext", nullable: true),
                    ReportIssuedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReportByUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReports", x => x.UserReportId);
                    table.ForeignKey(
                        name: "FK_UserReports_Users_ReportByUUID",
                        column: x => x.ReportByUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReports_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedReports_FeedPostId",
                table: "FeedReports",
                column: "FeedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedReports_ReportByUUID",
                table: "FeedReports",
                column: "ReportByUUID");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReportByUUID",
                table: "UserReports",
                column: "ReportByUUID");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_UserUUID",
                table: "UserReports",
                column: "UserUUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedReports");

            migrationBuilder.DropTable(
                name: "UserReports");
        }
    }
}
