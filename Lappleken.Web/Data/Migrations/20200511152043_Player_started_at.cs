using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Player_started_at : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivePlayerId",
                schema: "dbo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlayerStartedAt",
                schema: "dbo",
                table: "Game",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePlayerId",
                schema: "dbo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PlayerStartedAt",
                schema: "dbo",
                table: "Game");
        }
    }
}
