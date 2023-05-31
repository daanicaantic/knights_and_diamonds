using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class grave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GraveID",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GraveID",
                table: "CardInDecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Graves",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graves", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GraveID",
                table: "Games",
                column: "GraveID");

            migrationBuilder.CreateIndex(
                name: "IX_CardInDecks_GraveID",
                table: "CardInDecks",
                column: "GraveID");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInDecks_Graves_GraveID",
                table: "CardInDecks",
                column: "GraveID",
                principalTable: "Graves",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Graves_GraveID",
                table: "Games",
                column: "GraveID",
                principalTable: "Graves",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInDecks_Graves_GraveID",
                table: "CardInDecks");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Graves_GraveID",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Graves");

            migrationBuilder.DropIndex(
                name: "IX_Games_GraveID",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_CardInDecks_GraveID",
                table: "CardInDecks");

            migrationBuilder.DropColumn(
                name: "GraveID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GraveID",
                table: "CardInDecks");
        }
    }
}
