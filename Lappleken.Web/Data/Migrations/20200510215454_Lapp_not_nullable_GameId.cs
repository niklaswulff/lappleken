using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Lapp_not_nullable_GameId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lapp_Game_GameID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.AlterColumn<int>(
                name: "GameID",
                schema: "dbo",
                table: "Lapp",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lapp_Game_GameID",
                schema: "dbo",
                table: "Lapp",
                column: "GameID",
                principalSchema: "dbo",
                principalTable: "Game",
                principalColumn: "GameID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lapp_Game_GameID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.AlterColumn<int>(
                name: "GameID",
                schema: "dbo",
                table: "Lapp",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Lapp_Game_GameID",
                schema: "dbo",
                table: "Lapp",
                column: "GameID",
                principalSchema: "dbo",
                principalTable: "Game",
                principalColumn: "GameID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
