using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class Feed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PosterUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    PostType = table.Column<string>(type: "longtext", nullable: false),
                    PostCreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PostDescription = table.Column<string>(type: "longtext", nullable: true),
                    PostLikeCount = table.Column<int>(type: "int", nullable: false),
                    PostUrl = table.Column<string>(type: "longtext", nullable: true),
                    PollQuestion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPosts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_FeedPosts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedPosts_Users_PosterUUID",
                        column: x => x.PosterUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedPollAnswer",
                columns: table => new
                {
                    PollAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPollAnswer", x => x.PollAnswerId);
                    table.ForeignKey(
                        name: "FK_FeedPollAnswer_FeedPosts_PollId",
                        column: x => x.PollId,
                        principalTable: "FeedPosts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedPostComments",
                columns: table => new
                {
                    FeedCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: true),
                    IsAdminComment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPostComments", x => x.FeedCommentId);
                    table.ForeignKey(
                        name: "FK_FeedPostComments_FeedPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "FeedPosts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedPostComments_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedPostLikes",
                columns: table => new
                {
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    IsLiked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedPostLikes", x => new { x.PostId, x.UserUUID });
                    table.ForeignKey(
                        name: "FK_FeedPostLikes_FeedPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "FeedPosts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedPostLikes_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedAnswerVote",
                columns: table => new
                {
                    PollAnswerId = table.Column<int>(type: "int", nullable: false),
                    UserUUID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedAnswerVote", x => new { x.PollAnswerId, x.UserUUID });
                    table.ForeignKey(
                        name: "FK_FeedAnswerVote_FeedPollAnswer_PollAnswerId",
                        column: x => x.PollAnswerId,
                        principalTable: "FeedPollAnswer",
                        principalColumn: "PollAnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedAnswerVote_Users_UserUUID",
                        column: x => x.UserUUID,
                        principalTable: "Users",
                        principalColumn: "UserUUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedAnswerVote_UserUUID",
                table: "FeedAnswerVote",
                column: "UserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPollAnswer_PollId",
                table: "FeedPollAnswer",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPostComments_PostId",
                table: "FeedPostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPostComments_UserUUID",
                table: "FeedPostComments",
                column: "UserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPostLikes_UserUUID",
                table: "FeedPostLikes",
                column: "UserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPosts_PosterUUID",
                table: "FeedPosts",
                column: "PosterUUID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedPosts_TeamId",
                table: "FeedPosts",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedAnswerVote");

            migrationBuilder.DropTable(
                name: "FeedPostComments");

            migrationBuilder.DropTable(
                name: "FeedPostLikes");

            migrationBuilder.DropTable(
                name: "FeedPollAnswer");

            migrationBuilder.DropTable(
                name: "FeedPosts");
        }
    }
}
