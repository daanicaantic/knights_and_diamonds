using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class PlayerDeckChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Players_PlayerID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_PlayerID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                table: "CardInDecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardInDecks_PlayerID",
                table: "CardInDecks",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Players_PlayerID",
                table: "CardInDecks",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Players_PlayerID",
                table: "CardInDecks");

            migrationBuilder.DropIndex(
                name: "IX_CardInDecks_PlayerID",
                table: "CardInDecks");

            migrationBuilder.DropColumn(
                name: "PlayerID",
                table: "CardInDecks");

            migrationBuilder.AddColumn<int>(
                name: "PlayerID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PlayerID",
                table: "Cards",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Players_PlayerID",
                table: "Cards",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID");
        }
    }
}
