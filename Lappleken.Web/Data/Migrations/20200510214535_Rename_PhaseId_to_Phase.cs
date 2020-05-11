using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Rename_PhaseId_to_Phase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Phase",
                schema: "dbo",
                table: "Game",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("Update Game set Phase = PhaseId");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                schema: "dbo",
                table: "Game");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhaseId",
                schema: "dbo",
                table: "Game",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("Update Game set PhaseId = Phase");

            migrationBuilder.DropColumn(
                name: "Phase",
                schema: "dbo",
                table: "Game");

        }
    }
}
