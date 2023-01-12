using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class iniiail2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonsterCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpellTrapCards",
                table: "SpellTrapCards");

            migrationBuilder.RenameTable(
                name: "SpellTrapCards",
                newName: "Cards");

            migrationBuilder.AlterColumn<bool>(
                name: "SpellOrTrap",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "AttackPoints",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefencePoints",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MonsterType",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfStars",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AttackPoints",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "DefencePoints",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MonsterType",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "NumberOfStars",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "SpellTrapCards");

            migrationBuilder.AlterColumn<bool>(
                name: "SpellOrTrap",
                table: "SpellTrapCards",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpellTrapCards",
                table: "SpellTrapCards",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "MonsterCards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttackPoints = table.Column<int>(type: "int", nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefencePoints = table.Column<int>(type: "int", nullable: false),
                    Effect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonsterType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfStars = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterCards", x => x.ID);
                });
        }
    }
}
