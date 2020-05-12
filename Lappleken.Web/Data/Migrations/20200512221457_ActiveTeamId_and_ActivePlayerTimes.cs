using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class ActiveTeamId_and_ActivePlayerTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerStartedAt",
                schema: "dbo",
                table: "Game");

            migrationBuilder.AddColumn<bool>(
                name: "ActivePlayerDone",
                schema: "dbo",
                table: "Game",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActivePlayerRemainingTime",
                schema: "dbo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveTeamId",
                schema: "dbo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlayerEndsAt",
                schema: "dbo",
                table: "Game",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePlayerDone",
                schema: "dbo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "ActivePlayerRemainingTime",
                schema: "dbo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "ActiveTeamId",
                schema: "dbo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PlayerEndsAt",
                schema: "dbo",
                table: "Game");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlayerStartedAt",
                schema: "dbo",
                table: "Game",
                type: "datetime2",
                nullable: true);
        }
    }
}
