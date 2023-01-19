using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Cards_CardID",
                table: "CardInDecks");

            migrationBuilder.RenameColumn(
                name: "CardID",
                table: "CardInDecks",
                newName: "CardId");

            migrationBuilder.RenameIndex(
                name: "IX_CardInDecks_CardID",
                table: "CardInDecks",
                newName: "IX_CardInDecks_CardId");

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "CardInDecks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    isStillLogeniIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Cards_CardId",
                table: "CardInDecks",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Cards_CardId",
                table: "CardInDecks");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "CardInDecks",
                newName: "CardID");

            migrationBuilder.RenameIndex(
                name: "IX_CardInDecks_CardId",
                table: "CardInDecks",
                newName: "IX_CardInDecks_CardID");

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
        }
    }
}
