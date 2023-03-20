using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class UserIDinDeck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserID",
                table: "Decks");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Decks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserID",
                table: "Decks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_UserID",
                table: "Decks");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Decks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_UserID",
                table: "Decks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
