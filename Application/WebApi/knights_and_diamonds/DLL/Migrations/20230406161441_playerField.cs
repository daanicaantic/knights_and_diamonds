using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class playerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardField",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardOnFieldID = table.Column<int>(type: "int", nullable: true),
                    FieldIndex = table.Column<int>(type: "int", nullable: false),
                    PlayerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardField", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CardField_CardInDecks_CardOnFieldID",
                        column: x => x.CardOnFieldID,
                        principalTable: "CardInDecks",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CardField_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardField_CardOnFieldID",
                table: "CardField",
                column: "CardOnFieldID");

            migrationBuilder.CreateIndex(
                name: "IX_CardField_PlayerID",
                table: "CardField",
                column: "PlayerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardField");
        }
    }
}
