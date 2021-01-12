using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class FeedPollVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedAnswerVote_FeedPollAnswer_PollAnswerId",
                table: "FeedAnswerVote");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedAnswerVote_Users_UserUUID",
                table: "FeedAnswerVote");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedPollAnswer_FeedPosts_PollId",
                table: "FeedPollAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedPollAnswer",
                table: "FeedPollAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedAnswerVote",
                table: "FeedAnswerVote");

            migrationBuilder.RenameTable(
                name: "FeedPollAnswer",
                newName: "FeedPollAnswers");

            migrationBuilder.RenameTable(
                name: "FeedAnswerVote",
                newName: "FeedAnswerVotes");

            migrationBuilder.RenameIndex(
                name: "IX_FeedPollAnswer_PollId",
                table: "FeedPollAnswers",
                newName: "IX_FeedPollAnswers_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedAnswerVote_UserUUID",
                table: "FeedAnswerVotes",
                newName: "IX_FeedAnswerVotes_UserUUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedPollAnswers",
                table: "FeedPollAnswers",
                column: "PollAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedAnswerVotes",
                table: "FeedAnswerVotes",
                columns: new[] { "PollAnswerId", "UserUUID" });

            migrationBuilder.AddForeignKey(
                name: "FK_FeedAnswerVotes_FeedPollAnswers_PollAnswerId",
                table: "FeedAnswerVotes",
                column: "PollAnswerId",
                principalTable: "FeedPollAnswers",
                principalColumn: "PollAnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedAnswerVotes_Users_UserUUID",
                table: "FeedAnswerVotes",
                column: "UserUUID",
                principalTable: "Users",
                principalColumn: "UserUUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedPollAnswers_FeedPosts_PollId",
                table: "FeedPollAnswers",
                column: "PollId",
                principalTable: "FeedPosts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedAnswerVotes_FeedPollAnswers_PollAnswerId",
                table: "FeedAnswerVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedAnswerVotes_Users_UserUUID",
                table: "FeedAnswerVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedPollAnswers_FeedPosts_PollId",
                table: "FeedPollAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedPollAnswers",
                table: "FeedPollAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedAnswerVotes",
                table: "FeedAnswerVotes");

            migrationBuilder.RenameTable(
                name: "FeedPollAnswers",
                newName: "FeedPollAnswer");

            migrationBuilder.RenameTable(
                name: "FeedAnswerVotes",
                newName: "FeedAnswerVote");

            migrationBuilder.RenameIndex(
                name: "IX_FeedPollAnswers_PollId",
                table: "FeedPollAnswer",
                newName: "IX_FeedPollAnswer_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedAnswerVotes_UserUUID",
                table: "FeedAnswerVote",
                newName: "IX_FeedAnswerVote_UserUUID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedPollAnswer",
                table: "FeedPollAnswer",
                column: "PollAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedAnswerVote",
                table: "FeedAnswerVote",
                columns: new[] { "PollAnswerId", "UserUUID" });

            migrationBuilder.AddForeignKey(
                name: "FK_FeedAnswerVote_FeedPollAnswer_PollAnswerId",
                table: "FeedAnswerVote",
                column: "PollAnswerId",
                principalTable: "FeedPollAnswer",
                principalColumn: "PollAnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedAnswerVote_Users_UserUUID",
                table: "FeedAnswerVote",
                column: "UserUUID",
                principalTable: "Users",
                principalColumn: "UserUUID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedPollAnswer_FeedPosts_PollId",
                table: "FeedPollAnswer",
                column: "PollId",
                principalTable: "FeedPosts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
