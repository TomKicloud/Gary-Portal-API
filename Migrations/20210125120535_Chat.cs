using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class Chat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessageTypes",
                columns: table => new
                {
                    ChatMessageTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChatMessageTypeName = table.Column<string>(type: "longtext", nullable: true),
                    IsProtected = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageTypes", x => x.ChatMessageTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ChatName = table.Column<string>(type: "longtext", nullable: true),
                    ChatIsProtected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ChatIsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ChatIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ChatCreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatUUID);
                });

            migrationBuilder.CreateTable(
                name: "ChatMembers",
                columns: table => new
                {
                    ChatUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsInChat = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMembers", x => new { x.ChatUUID, x.UserUUID });
                    table.ForeignKey(
                        name: "FK_ChatMembers_Chats_ChatUUID",
                        column: x => x.ChatUUID,
                        principalTable: "Chats",
                        principalColumn: "ChatUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMembers_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    ChatMessageUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    ChatUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    MessageContent = table.Column<string>(type: "longtext", nullable: true),
                    MessageCreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MessageHasBeenEdited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MessageTypeId = table.Column<int>(type: "int", nullable: false),
                    MessageIsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.ChatMessageUUID);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatMessageTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "ChatMessageTypes",
                        principalColumn: "ChatMessageTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Chats_ChatUUID",
                        column: x => x.ChatUUID,
                        principalTable: "Chats",
                        principalColumn: "ChatUUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_UserUUID",
                table: "ChatMembers",
                column: "UserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatUUID",
                table: "ChatMessages",
                column: "ChatUUID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_MessageTypeId",
                table: "ChatMessages",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserUUID",
                table: "ChatMessages",
                column: "UserUUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMembers");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatMessageTypes");

            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
