using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class iniial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonsterCards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfStars = table.Column<int>(type: "int", nullable: false),
                    AttackPoints = table.Column<int>(type: "int", nullable: false),
                    DefencePoints = table.Column<int>(type: "int", nullable: false),
                    MonsterType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterCards", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SpellTrapCards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpellOrTrap = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellTrapCards", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonsterCards");

            migrationBuilder.DropTable(
                name: "SpellTrapCards");
        }
    }
}
