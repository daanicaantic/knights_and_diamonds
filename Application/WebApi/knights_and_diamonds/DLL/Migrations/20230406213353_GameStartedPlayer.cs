using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class GameStartedPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameStarted",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "GaemeStarted",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GaemeStarted",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "GameStarted",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
