using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class PlayersHandChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_PlayerHands_PlayerHandID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_PlayerHandID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PlayerHandID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "PlayersHandID",
                table: "CardInDecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardInDecks_PlayersHandID",
                table: "CardInDecks",
                column: "PlayersHandID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_PlayerHands_PlayersHandID",
                table: "CardInDecks",
                column: "PlayersHandID",
                principalTable: "PlayerHands",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_PlayerHands_PlayersHandID",
                table: "CardInDecks");

            migrationBuilder.DropIndex(
                name: "IX_CardInDecks_PlayersHandID",
                table: "CardInDecks");

            migrationBuilder.DropColumn(
                name: "PlayersHandID",
                table: "CardInDecks");

            migrationBuilder.AddColumn<int>(
                name: "PlayerHandID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PlayerHandID",
                table: "Cards",
                column: "PlayerHandID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_PlayerHands_PlayerHandID",
                table: "Cards",
                column: "PlayerHandID",
                principalTable: "PlayerHands",
                principalColumn: "ID");
        }
    }
}
