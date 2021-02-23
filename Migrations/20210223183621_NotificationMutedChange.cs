using Microsoft.EntityFrameworkCore.Migrations;

namespace GaryPortalAPI.Migrations
{
    public partial class NotificationMutedChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMuted",
                table: "UserAPNS");

            migrationBuilder.AddColumn<bool>(
                name: "NotificationsMuted",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationsMuted",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsMuted",
                table: "UserAPNS",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
