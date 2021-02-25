using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class EmailConfirmandPassResets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAuthConfirmations",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserConfirmationHash = table.Column<string>(type: "varchar(255)", nullable: false),
                    ConfirmationExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConfirmationIsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthConfirmations", x => new { x.UserUUID, x.UserConfirmationHash });
                    table.ForeignKey(
                        name: "FK_UserAuthConfirmations_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPassResetTokens",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserResetHash = table.Column<string>(type: "varchar(255)", nullable: false),
                    HashExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HashIsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPassResetTokens", x => new { x.UserUUID, x.UserResetHash });
                    table.ForeignKey(
                        name: "FK_UserPassResetTokens_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthConfirmations");

            migrationBuilder.DropTable(
                name: "UserPassResetTokens");
        }
    }
}
