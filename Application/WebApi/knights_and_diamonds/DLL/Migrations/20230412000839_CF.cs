using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class CF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardField_CardInDecks_CardOnFieldID",
                table: "CardField");

            migrationBuilder.DropForeignKey(
                name: "FK_CardField_Players_PlayerID",
                table: "CardField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardField",
                table: "CardField");

            migrationBuilder.RenameTable(
                name: "CardField",
                newName: "CardFields");

            migrationBuilder.RenameIndex(
                name: "IX_CardField_PlayerID",
                table: "CardFields",
                newName: "IX_CardFields_PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_CardField_CardOnFieldID",
                table: "CardFields",
                newName: "IX_CardFields_CardOnFieldID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardFields",
                table: "CardFields",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardFields_CardInDecks_CardOnFieldID",
                table: "CardFields",
                column: "CardOnFieldID",
                principalTable: "CardInDecks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardFields_Players_PlayerID",
                table: "CardFields",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardFields_CardInDecks_CardOnFieldID",
                table: "CardFields");

            migrationBuilder.DropForeignKey(
                name: "FK_CardFields_Players_PlayerID",
                table: "CardFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardFields",
                table: "CardFields");

            migrationBuilder.RenameTable(
                name: "CardFields",
                newName: "CardField");

            migrationBuilder.RenameIndex(
                name: "IX_CardFields_PlayerID",
                table: "CardField",
                newName: "IX_CardField_PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_CardFields_CardOnFieldID",
                table: "CardField",
                newName: "IX_CardField_CardOnFieldID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardField",
                table: "CardField",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardField_CardInDecks_CardOnFieldID",
                table: "CardField",
                column: "CardOnFieldID",
                principalTable: "CardInDecks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardField_Players_PlayerID",
                table: "CardField",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
