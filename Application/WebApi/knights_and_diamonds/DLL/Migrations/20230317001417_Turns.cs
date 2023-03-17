using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class Turns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_UserHandID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Hands");

            migrationBuilder.RenameColumn(
                name: "UserHandID",
                table: "Cards",
                newName: "PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_UserHandID",
                table: "Cards",
                newName: "IX_Cards_PlayerID");

            migrationBuilder.AddColumn<int>(
                name: "HandID",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LifePoints",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerHandID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlayerHands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerHands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrawPhase = table.Column<bool>(type: "bit", nullable: false),
                    MainPhase = table.Column<bool>(type: "bit", nullable: false),
                    BattlePhase = table.Column<bool>(type: "bit", nullable: false),
                    EndPhase = table.Column<bool>(type: "bit", nullable: false),
                    MonsterSummoned = table.Column<bool>(type: "bit", nullable: false),
                    TurnNumber = table.Column<int>(type: "int", nullable: false),
                    PlayerOnTurn = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Turns_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_HandID",
                table: "Players",
                column: "HandID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PlayerHandID",
                table: "Cards",
                column: "PlayerHandID");

            migrationBuilder.CreateIndex(
                name: "IX_Turns_GameID",
                table: "Turns",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_PlayerHands_PlayerHandID",
                table: "Cards",
                column: "PlayerHandID",
                principalTable: "PlayerHands",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Players_PlayerID",
                table: "Cards",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerHands_HandID",
                table: "Players",
                column: "HandID",
                principalTable: "PlayerHands",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_PlayerHands_PlayerHandID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Players_PlayerID",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerHands_HandID",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PlayerHands");

            migrationBuilder.DropTable(
                name: "Turns");

            migrationBuilder.DropIndex(
                name: "IX_Players_HandID",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Cards_PlayerHandID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "HandID",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LifePoints",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerHandID",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "PlayerID",
                table: "Cards",
                newName: "UserHandID");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_PlayerID",
                table: "Cards",
                newName: "IX_Cards_UserHandID");

            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hands_Decks_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Decks",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Hands_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hands_DeckID",
                table: "Hands",
                column: "DeckID");

            migrationBuilder.CreateIndex(
                name: "IX_Hands_UserID",
                table: "Hands",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_UserHandID",
                table: "Cards",
                column: "UserHandID",
                principalTable: "Hands",
                principalColumn: "ID");
        }
    }
}
