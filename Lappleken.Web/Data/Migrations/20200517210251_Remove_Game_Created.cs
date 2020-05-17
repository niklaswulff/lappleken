using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Remove_Game_Created : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                schema: "dbo",
                table: "Game");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Created",
                schema: "dbo",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
