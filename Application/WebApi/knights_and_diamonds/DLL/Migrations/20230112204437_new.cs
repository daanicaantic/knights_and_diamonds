using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cardtype",
                table: "Cards",
                newName: "CardType");

            migrationBuilder.AddColumn<string>(
                name: "ElementType",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElementType",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "CardType",
                table: "Cards",
                newName: "cardtype");
        }
    }
}
