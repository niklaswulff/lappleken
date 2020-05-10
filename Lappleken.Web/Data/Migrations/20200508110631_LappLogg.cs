using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lappleken.Web.Data.Migrations
{
    public partial class LappLogg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LappLogg",
                columns: table => new
                {
                    LappLoggID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LappID = table.Column<int>(nullable: false),
                    ActionByPlayerID = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Phase = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LappLogg", x => x.LappLoggID);
                    table.ForeignKey(
                        name: "FK_LappLogg_Player_ActionByPlayerID",
                        column: x => x.ActionByPlayerID,
                        principalSchema: "dbo",
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LappLogg_Lapp_LappID",
                        column: x => x.LappID,
                        principalSchema: "dbo",
                        principalTable: "Lapp",
                        principalColumn: "LappID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LappLogg_ActionByPlayerID",
                table: "LappLogg",
                column: "ActionByPlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_LappLogg_LappID",
                table: "LappLogg",
                column: "LappID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LappLogg");
        }
    }
}
