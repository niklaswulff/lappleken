using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Phase_started : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PhaseStarted",
                schema: "dbo",
                table: "Game",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseStarted",
                schema: "dbo",
                table: "Game");
        }
    }
}
