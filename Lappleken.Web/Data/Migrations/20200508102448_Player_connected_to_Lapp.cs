using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Player_connected_to_Lapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lapp_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                column: "CreatedByPlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                column: "CreatedByPlayerID",
                principalSchema: "dbo",
                principalTable: "Player",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.DropIndex(
                name: "IX_Lapp_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.DropColumn(
                name: "CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp");
        }
    }
}
