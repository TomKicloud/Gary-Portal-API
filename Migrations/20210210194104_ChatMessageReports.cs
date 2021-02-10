using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class ChatMessageReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessageReports",
                columns: table => new
                {
                    ChatMessageReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatMessageUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ReportReason = table.Column<string>(type: "longtext", nullable: true),
                    ReportIssuedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReportByUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageReports", x => x.ChatMessageReportId);
                    table.ForeignKey(
                        name: "FK_ChatMessageReports_ChatMessages_ChatMessageUUID",
                        column: x => x.ChatMessageUUID,
                        principalTable: "ChatMessages",
                        principalColumn: "ChatMessageUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessageReports_Users_ReportByUUID",
                        column: x => x.ReportByUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageReports_ChatMessageUUID",
                table: "ChatMessageReports",
                column: "ChatMessageUUID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageReports_ReportByUUID",
                table: "ChatMessageReports",
                column: "ReportByUUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessageReports");
        }
    }
}
