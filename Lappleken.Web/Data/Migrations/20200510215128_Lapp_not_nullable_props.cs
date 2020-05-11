using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class Lapp_not_nullable_props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Lapp",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                column: "CreatedByPlayerID",
                principalSchema: "dbo",
                principalTable: "Player",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Lapp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Lapp_Player_CreatedByPlayerID",
                schema: "dbo",
                table: "Lapp",
                column: "CreatedByPlayerID",
                principalSchema: "dbo",
                principalTable: "Player",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
