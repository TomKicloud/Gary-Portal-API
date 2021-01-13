using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class UserBlocksBans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PostIsGlobal",
                table: "FeedPosts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BanTypes",
                columns: table => new
                {
                    BanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BanTypeName = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanTypes", x => x.BanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserBlocks",
                columns: table => new
                {
                    BlockerUserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    BlockedUserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlocks", x => new { x.BlockerUserUUID, x.BlockedUserUUID });
                    table.ForeignKey(
                        name: "FK_UserBlocks_Users_BlockedUserUUID",
                        column: x => x.BlockedUserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBlocks_Users_BlockerUserUUID",
                        column: x => x.BlockerUserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBans",
                columns: table => new
                {
                    UserBanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    BanIssued = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BanExpires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BanTypeId = table.Column<int>(type: "int", nullable: false),
                    BanReason = table.Column<string>(type: "longtext", nullable: true),
                    BannedByUUID = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBans", x => x.UserBanId);
                    table.ForeignKey(
                        name: "FK_UserBans_BanTypes_BanTypeId",
                        column: x => x.BanTypeId,
                        principalTable: "BanTypes",
                        principalColumn: "BanTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBans_Users_BannedByUUID",
                        column: x => x.BannedByUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBans_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBans_BannedByUUID",
                table: "UserBans",
                column: "BannedByUUID");

            migrationBuilder.CreateIndex(
                name: "IX_UserBans_BanTypeId",
                table: "UserBans",
                column: "BanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBans_UserUUID",
                table: "UserBans",
                column: "UserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlocks_BlockedUserUUID",
                table: "UserBlocks",
                column: "BlockedUserUUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBans");

            migrationBuilder.DropTable(
                name: "UserBlocks");

            migrationBuilder.DropTable(
                name: "BanTypes");

            migrationBuilder.DropColumn(
                name: "PostIsGlobal",
                table: "FeedPosts");
        }
    }
}
