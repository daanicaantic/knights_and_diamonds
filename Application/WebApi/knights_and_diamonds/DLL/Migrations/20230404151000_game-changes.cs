using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class gamechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerOnTurn",
                table: "Turns");

            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "Turns");

            migrationBuilder.AddColumn<int>(
                name: "PlayerOnTurn",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerOnTurn",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "PlayerOnTurn",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "Turns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
