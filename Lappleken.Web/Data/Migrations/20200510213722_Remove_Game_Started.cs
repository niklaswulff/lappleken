using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Remove_Game_Started : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update Game Set PhaseId = 1 WHERE Started = 1");

            migrationBuilder.DropColumn(
                name: "Started",
                schema: "dbo",
                table: "Game");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Started",
                schema: "dbo",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
