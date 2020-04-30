using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Game",
                schema: "dbo",
                columns: table => new
                {
                    GameID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Started = table.Column<bool>(nullable: false),
                    Created = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameID);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                schema: "dbo",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Lapp",
                schema: "dbo",
                columns: table => new
                {
                    LappID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    GameID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lapp", x => x.LappID);
                    table.ForeignKey(
                        name: "FK_Lapp_Game_GameID",
                        column: x => x.GameID,
                        principalSchema: "dbo",
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "dbo",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    GameID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamID);
                    table.ForeignKey(
                        name: "FK_Team_Game_GameID",
                        column: x => x.GameID,
                        principalSchema: "dbo",
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lapp_GameID",
                schema: "dbo",
                table: "Lapp",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_GameID",
                schema: "dbo",
                table: "Team",
                column: "GameID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lapp",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Player",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Team",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Game",
                schema: "dbo");
        }
    }
}
