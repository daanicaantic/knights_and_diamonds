using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class effect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Cards_CardID",
                table: "CardInDecks");

            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Decks_DeckID",
                table: "CardInDecks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "DeckID",
                table: "CardInDecks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CardID",
                table: "CardInDecks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Cards_CardID",
                table: "CardInDecks",
                column: "CardID",
                principalTable: "Cards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Decks_DeckID",
                table: "CardInDecks",
                column: "DeckID",
                principalTable: "Decks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Cards_CardID",
                table: "CardInDecks");

            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Decks_DeckID",
                table: "CardInDecks");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeckID",
                table: "CardInDecks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CardID",
                table: "CardInDecks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Cards_CardID",
                table: "CardInDecks",
                column: "CardID",
                principalTable: "Cards",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Decks_DeckID",
                table: "CardInDecks",
                column: "DeckID",
                principalTable: "Decks",
                principalColumn: "ID");
        }
    }
}
