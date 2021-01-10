using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class InitialUserCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RankName = table.Column<string>(type: "longtext", nullable: true),
                    RankAccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(type: "longtext", nullable: true),
                    TeamAccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserFullName = table.Column<string>(type: "longtext", nullable: true),
                    UserSpanishName = table.Column<string>(type: "longtext", nullable: true),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    UserProfileImageUrl = table.Column<string>(type: "longtext", nullable: true),
                    UserQuote = table.Column<string>(type: "longtext", nullable: true),
                    UserBio = table.Column<string>(type: "longtext", nullable: true),
                    UserIsStaff = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserIsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserStanding = table.Column<string>(type: "longtext", nullable: true),
                    IsQueued = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserUUID);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthentications",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserEmail = table.Column<string>(type: "longtext", nullable: true),
                    UserPhone = table.Column<string>(type: "longtext", nullable: true),
                    UserEmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserPhoneConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserPassHash = table.Column<string>(type: "longtext", nullable: true),
                    UserPassSalt = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthentications", x => x.UserUUID);
                    table.ForeignKey(
                        name: "FK_UserAuthentications_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPoints",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    AmigoPoints = table.Column<int>(type: "int", nullable: false),
                    PositivityPoints = table.Column<int>(type: "int", nullable: false),
                    BowelsRelieved = table.Column<int>(type: "int", nullable: false),
                    Prayers = table.Column<int>(type: "int", nullable: false),
                    MeaningfulPrayers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPoints", x => x.UserUUID);
                    table.ForeignKey(
                        name: "FK_UserPoints_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRanks",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    AmigoRankId = table.Column<int>(type: "int", nullable: false),
                    PositivtyRankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRanks", x => x.UserUUID);
                    table.ForeignKey(
                        name: "FK_UserRanks_Ranks_AmigoRankId",
                        column: x => x.AmigoRankId,
                        principalTable: "Ranks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRanks_Ranks_PositivtyRankId",
                        column: x => x.PositivtyRankId,
                        principalTable: "Ranks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRanks_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(255)", nullable: false),
                    TokenIssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TokenExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TokenClient = table.Column<string>(type: "longtext", nullable: true),
                    TokenIsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => new { x.UserUUID, x.RefreshToken });
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => x.UserUUID);
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRanks_AmigoRankId",
                table: "UserRanks",
                column: "AmigoRankId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRanks_PositivtyRankId",
                table: "UserRanks",
                column: "PositivtyRankId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthentications");

            migrationBuilder.DropTable(
                name: "UserPoints");

            migrationBuilder.DropTable(
                name: "UserRanks");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
