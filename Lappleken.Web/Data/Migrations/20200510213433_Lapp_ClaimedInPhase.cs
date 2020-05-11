using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Lapp_ClaimedInPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimedInPhase",
                schema: "dbo",
                table: "Lapp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "dbo",
                table: "Game",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PhaseId",
                schema: "dbo",
                table: "Game",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimedInPhase",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "dbo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                schema: "dbo",
                table: "Game");
        }
    }
}
