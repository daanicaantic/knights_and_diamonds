using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class etype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EffectID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EffectTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffectTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Effects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EffectTypeID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOfCardsAffected = table.Column<int>(type: "int", nullable: false),
                    PointsAddedLost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Effects_EffectTypes_EffectTypeID",
                        column: x => x.EffectTypeID,
                        principalTable: "EffectTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_EffectID",
                table: "Cards",
                column: "EffectID");

            migrationBuilder.CreateIndex(
                name: "IX_Effects_EffectTypeID",
                table: "Effects",
                column: "EffectTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Effects_EffectID",
                table: "Cards",
                column: "EffectID",
                principalTable: "Effects",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Effects_EffectID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Effects");

            migrationBuilder.DropTable(
                name: "EffectTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cards_EffectID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "EffectID",
                table: "Cards");
        }
    }
}
