using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class PlayerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                schema: "dbo",
                table: "Player",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                schema: "dbo",
                table: "Player",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                schema: "dbo",
                table: "Player",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamID",
                schema: "dbo",
                table: "Player",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamID",
                schema: "dbo",
                table: "Player",
                column: "TeamID",
                principalSchema: "dbo",
                principalTable: "Team",
                principalColumn: "TeamID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Team_TeamID",
                schema: "dbo",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                schema: "dbo",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_TeamID",
                schema: "dbo",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                schema: "dbo",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "TeamID",
                schema: "dbo",
                table: "Player");
        }
    }
}
