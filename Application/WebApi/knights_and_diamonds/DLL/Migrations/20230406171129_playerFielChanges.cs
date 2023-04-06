using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class playerFielChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CardPosition",
                table: "CardField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CardShowen",
                table: "CardField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FieldType",
                table: "CardField",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardPosition",
                table: "CardField");

            migrationBuilder.DropColumn(
                name: "CardShowen",
                table: "CardField");

            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "CardField");
        }
    }
}
