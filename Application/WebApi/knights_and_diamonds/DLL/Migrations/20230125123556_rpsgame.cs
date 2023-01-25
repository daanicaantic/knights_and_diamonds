using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class rpsgame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreGameGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.CreateTable(
                name: "RockPaperScissorsGames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockPaperScissorsGames", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PreGameSessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Play = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    RPSGameID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreGameSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreGameSessions_RockPaperScissorsGames_RPSGameID",
                        column: x => x.RPSGameID,
                        principalTable: "RockPaperScissorsGames",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PreGameSessions_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreGameSessions_RPSGameID",
                table: "PreGameSessions",
                column: "RPSGameID");

            migrationBuilder.CreateIndex(
                name: "IX_PreGameSessions_UserID",
                table: "PreGameSessions",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreGameSessions");

            migrationBuilder.DropTable(
                name: "RockPaperScissorsGames");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PreGameGames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Play = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreGameGames", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreGameGames_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PreGameGames_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGames_GameID",
                table: "PreGameGames",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGames_UserID",
                table: "PreGameGames",
                column: "UserID");
        }
    }
}
