using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AttackInTurn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttackInTurns",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAttackingDone = table.Column<bool>(type: "bit", nullable: false),
                    TurnID = table.Column<int>(type: "int", nullable: false),
                    CardFieldID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackInTurns", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AttackInTurns_CardFields_CardFieldID",
                        column: x => x.CardFieldID,
                        principalTable: "CardFields",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttackInTurns_Turns_TurnID",
                        column: x => x.TurnID,
                        principalTable: "Turns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttackInTurns_CardFieldID",
                table: "AttackInTurns",
                column: "CardFieldID");

            migrationBuilder.CreateIndex(
                name: "IX_AttackInTurns_TurnID",
                table: "AttackInTurns",
                column: "TurnID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttackInTurns");
        }
    }
}
